using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandRename : CommandWithGenerationBase
    {
        public const string Name = "/rename";

        public CommandRename(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using (var db = new DataContext())
            {
                var currentRole = await db.CurrentRoles.FirstOrDefaultAsync(cr => cr.ChatId == ChatId);
                if (currentRole is null)
                {
                    currentRole = await db.AcceptRoleAsync(ChatId, 1);
                }

                currentRole.Username = _data!.ToString()!;

                await db.SaveChangesAsync();

                await _bot.SendTextMessageAsync(ChatId, "Ваше имя в диалоге успешно изменено");
            }
        }
    }
}
