using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Requests;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandRuOnly : TgAction<MessageHandler>
    {
        public const string Name = "/ru_only";
        public CommandRuOnly(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            var cr = await db.CurrentRoles.FirstOrDefaultAsync(cr => cr.ChatId == ChatId);

            if (cr is not null)
            {
                cr.RuOnly = !cr.RuOnly;
            }
            else
            {
                cr = new DbCurrentRole
                {
                    ChatId = ChatId,
                    RoleId = 1,
                    RuOnly = true
                };

                await db.CurrentRoles.AddAsync(cr);
            }

            await db.SaveChangesAsync();

            await _bot.SendTextMessageAsync(ChatId, cr.RuOnly ? "Теперь модель избегает английских слов" : "Теперь английские слова в ответе допустимы");
        }
    }
}
