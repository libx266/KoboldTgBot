using KoboldTgBot.Database;
using KoboldTgBot.Neuro;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackRole : TgCallbackBase
    {
        public CallbackRole(ITelegramBotClient bot, CallbackQuery callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            long chatId = _callback.Message.Chat.Id;
            int roleId = Int32.TryParse(Data, out int r) ? r : 1;

            var currentRole = await db.CurrentRoles.FirstOrDefaultAsync(cr => cr.ChatId == chatId);
            if (currentRole is null)
            {
                await db.CurrentRoles.AddAsync(new DbCurrentRole
                {
                    ChatId = chatId,
                    RoleId = roleId
                });
            }
            else
            {
                currentRole.RoleId = roleId;
            }

            await db.SaveChangesAsync();

            await _bot.SendTextMessageAsync(_callback.Message.Chat.Id, "Применена роль:  " + await db.Roles.Where(r => r.ID == roleId).Select(r => r.Name).FirstOrDefaultAsync());
            await _bot.DeleteMessageAsync(_callback.Message.Chat.Id, _callback.Message.MessageId);
        }
    }
}
