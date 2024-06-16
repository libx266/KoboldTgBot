using KoboldTgBot.Database;
using KoboldTgBot.Neuro;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace KoboldTgBot.Utils
{
    internal static class Extensions
    {
        internal static void Log(this Exception ex) =>
            Task.Run(() => Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented)));

        internal static async Task<string> ConstructPropmptAsync(this DataContext db, long chatId, User? user, NeuroCharacterRoleManager? roles)
        {
            var messages = await db.Messages.Where(m => m.ChatId == chatId && m.InMemory).OrderByDescending(m => m.ID).Take(byte.MaxValue).ToListAsync();

            string senderName;

            if (string.IsNullOrWhiteSpace(senderName = user!.FirstName ?? "" + ' ' + user.LastName ?? ""))
            {
                senderName = user.Username ?? "Anonymous";
            }

            var actualFilteredMessages =
            (
                from m in messages
                group m by m.TgId into mg
                let m2 = mg.MaxBy(m => m.ID)
                orderby m2.ID descending
                select new
                {
                    Text = m2.Text,
                    Sender = m2.SenderId
                }
            );

            var dialog = new List<string>();
            int count = 0;

            foreach (var m in actualFilteredMessages)
            {
                string row = $"{new[] { senderName, Properties.Resources.BotName }[Convert.ToInt32(m.Sender == -1)]}:  {m.Text}";

                count += row.Length;

                if (count > 21504)
                {
                    break;
                }

                dialog.Add(row);
            }

            dialog.Reverse();

            return String.Format(Properties.Resources.NeuroCharacterPrompt, roles!.GetPrompt(chatId), String.Join("\n", dialog.Append($"{Properties.Resources.BotName}:  ")));
        }


        private static IEnumerable<char> FilterEmojis(string text)
        {
            foreach(var c in text)
            {
                if
                (
                    (c >= 0x400 && c <= 0x491) || // Кириллица
                             c <= 0x7E         ||   // ASCII
                    (c >= 0xA1 && c <= 0xFF)      //дополнения к латинице
                )
                {
                    yield return c;
                }
            }
        }

        public static string RemoveEmojis(string text) => 
            String.Join("", FilterEmojis(text));
            
    }
}
