using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.Utils;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace KoboldTgBot.Extensions.Utils
{
    internal static class LLMProcessingHelper
    {
        internal static async Task<PromptDto> ConstructPropmptAsync(this DataContext db, long chatId, User? user)
        {
            string senderName;

            if (string.IsNullOrWhiteSpace(senderName = user!.FirstName ?? "" + ' ' + user.LastName ?? ""))
            {
                senderName = user.Username ?? "Anonymous";
            }

            var dialog = new List<string>();
            int count = 0;

            var role = await db.GetCurrentRoleAsync(chatId);

            string prompt = String.Format(ConfigurationManager.PromptTemplate, String.Format
            (
                Properties.Resources.NeuroCharacterPrompt,
                role.Name,
                role.Gender,
                role.Character,
                role.Specialisation,
                role.Relation,
                role.Style
            ));

            int max = Convert.ToInt32((ConfigurationManager.MaxContextLength - (ConfigurationManager.MaxGenerationLength + prompt.Length / ConfigurationManager.AverageSymbolsPerToken)) * ConfigurationManager.AverageSymbolsPerToken);

            foreach (var m in await db.GetMessagesShortFilteredListAsync(chatId, role.ID))
            {
                string row = $"{new[] { senderName, role.Name }[Convert.ToInt32(m.Sender == -1)]}:  {m.Text}";

                count += row.Length;

                if (count > max)
                {
                    break;
                }

                dialog.Add(row);
            }

            dialog.Reverse();

            return new PromptDto(prompt.Replace("@{dialog}", String.Join("\n", dialog.Append($"{role.Name}:  "))), role.Name, senderName);
        }

        private static IEnumerable<char> FilterEmojis(string text)
        {
            foreach (var c in text)
            {
                if
                (
                    (c >= 0x400 && c <= 0x491) || // Кириллица
                             c <= 0x7E         || // ASCII
                    (c >= 0xA1 && c <= 0xFF)      //дополнения к латинице
                )
                {
                    yield return c;
                }
            }
        }

        internal static string RemoveEmojis(string text) =>
            String.Join(default(string), FilterEmojis(text));

        private static int GetLastApostrophIndex(string text, out int count)
        {
            count = 0;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                if (c == '`')
                {
                    count += 3;
                    return i - 2;
                }
                count++;
            }
            return -1;
        }

        internal static string? Filter(string? text, string[] stop)
        {
            var check = () =>
            {
                if (string.IsNullOrEmpty(text) || text.Length < 3)
                {
                    throw new Exception("Empty text");
                }
            };

            try
            {
                check();

                stop.ToList().ForEach(s => text = text!.Replace(s, ""));
                check();

                if (text!.StartsWith("```"))
                {
                    text = text.Remove(0, 3);
                }
                check();

                if (Convert.ToBoolean(Regex.Matches(text, @"```").Count % 2))
                {
                    text = text.Remove(GetLastApostrophIndex(text, out int count), count);
                    check();
                }

                return text;
            }
            catch
            {
                return default;
            }

        }
    }
}
