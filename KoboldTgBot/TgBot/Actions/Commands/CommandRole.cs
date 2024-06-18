using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandRole : TgAction<MessageHandler>
    {
        public const string Name = "/role";

        public CommandRole(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            long userId = UserId;

            var roles = await
            (
                from r in db.Roles
                where r.UserId == userId ||
                r.UserId == -1
                select new
                {
                    r.ID,
                    r.Title
                }
            ).ToListAsync();

            var buttons = roles.Select(r => new InlineKeyboardButton[] { Extensions.MakeInlineButton<CallbackRole>(r.Title, r.ID) });

            var keyboard = new InlineKeyboardMarkup(buttons.Append(new InlineKeyboardButton[] { Extensions.MakeInlineButton<CallbackCreateRole>("Добавить") }));

            await _bot.SendTextMessageAsync(ChatId, "Выберите роль персонажа:", replyMarkup: keyboard);
        }
    }
}
