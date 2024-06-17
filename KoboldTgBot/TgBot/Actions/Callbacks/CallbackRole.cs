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
using Telegram.Bot.Types.ReplyMarkups;

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

            int roleId = Int32.TryParse(Data, out int r) ? r : 1;

            var role = await db.Roles.FirstAsync(r => r.ID == roleId);

            var buttons = new List<InlineKeyboardButton> { new InlineKeyboardButton("Применить") { CallbackData = "accept_role=" + roleId } };

            var info = String.Format
            (
                Properties.Resources.RoleViewInfo,
                role.Name,
                role.Gender,
                role.Charakter,
                role.Specialisation,
                role.Relation,
                role.Style
            );

            if (role.UserId != -1L)
            {
                buttons.Add(new InlineKeyboardButton("Удалить") { CallbackData = "delete_role=" + roleId });
            }

            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][] { buttons.ToArray() });

            await _bot.EditMessageTextAsync(_callback.Message!.Chat.Id, _callback.Message.MessageId, info, replyMarkup: keyboard);
        }
    }
}
