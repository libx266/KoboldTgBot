using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace KoboldTgBot.Utils
{
    internal static class Extensions
    {
        internal static void Log(this Exception ex) =>
            Task.Run(() => Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented)));

        internal static async Task<string> ConstructPropmptAsync(this DataContext db, long chatId, User user)
        {
            var messages = await db.Messages.Where(m => m.ChatId == chatId && m.InMemory).OrderByDescending(m => m.ID).Take(byte.MaxValue).ToListAsync();

            string senderName;

            if (string.IsNullOrWhiteSpace(senderName = user.FirstName ?? "" + ' ' + user.LastName ?? ""))
            {
                senderName = user.Username ?? "Anonymous";
            }

            var actualFilteredMessages =
            (
                from m in messages
                group m by m.TgId into mg
                let m2 = mg.OrderByDescending(m => m.ID).First()
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
                if (count > 16384)
                {
                    break;
                }

                string row = $"{new[] { senderName, Properties.Resources.BotName }[Convert.ToInt32(m.Sender == -1)]}:  {m.Text}";

                dialog.Add(row);
                count += row.Length;
            }

            dialog.Reverse();

            return String.Format(Properties.Resources.NeuroCharacterPrompt, String.Join("\n", dialog.Append($"{Properties.Resources.BotName}:  ")));
        }
    }
}
