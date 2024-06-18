using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackRole : TgAction<CallbackHandler>
    {
        public const string Name = "role";

        public CallbackRole(ITelegramBotClient bot, CallbackHandler callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            int roleId = Int32.TryParse(Entity.Data, out int r) ? r : 1;

            var role = await db.Roles.FirstAsync(r => r.ID == roleId);

            var buttons = new List<InlineKeyboardButton> { Extensions.MakeInlineButton<CallbackAcceptRole>("Применить", roleId) };

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
                buttons.Add(Extensions.MakeInlineButton<CallbackDeleteRole>("Удалить", roleId));
            }

            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][] { buttons.ToArray() });

            await _bot.EditMessageTextAsync(ChatId, MessageId, info, replyMarkup: keyboard);
        }
    }
}
