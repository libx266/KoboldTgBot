using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
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


            foreach (var m in await db.GetMessagesShortFilteredList(chatId))
            {
                string row = $"{new[] { senderName, role.Name }[Convert.ToInt32(m.Sender == -1)]}:  {m.Text}";

                count += row.Length;

                if (count > 21504)
                {
                    break;
                }

                dialog.Add(row);
            }

            dialog.Reverse();

            string prompt = String.Format
            (
                Properties.Resources.NeuroCharacterPrompt,
                role.Name,
                role.Gender,
                role.Character,
                role.Specialisation,
                role.Relation,
                role.Style,
                String.Join("\n", dialog.Append($"{role.Name}:  "))
            );

            return new PromptDto(prompt, role.Name, senderName);
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
                    var segments = text.Split("```");

                    string? last = segments.LastOrDefault();

                    if(!string.IsNullOrWhiteSpace(last) && segments.Length > 1)
                    {
                        text = text.Replace(last, "");
                        check(); 
                    }

                    text = text.Remove(text.Length - 5, 4);
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
