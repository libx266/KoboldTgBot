using KoboldTgBot.Database;
using KoboldTgBot.Neuro;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace KoboldTgBot.Extensions
{
    internal static class PromptHelper
    {
        internal static async Task<PromptDto> ConstructPropmptAsync(this DataContext db, long chatId, User? user)
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
                    Sender = m2.UserId
                }
            );

            var dialog = new List<string>();
            int count = 0;

            var role = await
            (
                from cr in db.CurrentRoles.Where(cr => cr.ChatId == chatId).DefaultIfEmpty()
                from r in db.Roles
                where r.ID == (cr == default ? 1 : cr.RoleId)
                select r
            ).FirstAsync();

            foreach (var m in actualFilteredMessages)
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
                role.Charakter,
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
    }
}
