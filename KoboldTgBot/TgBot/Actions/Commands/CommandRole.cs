using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandRole : TgCommandBase
    {
        public CommandRole(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            long userId = _message.From!.Id;

            var roles = await
            (
                from r in db.Roles
                where r.UserId == userId ||
                r.UserId == -1
                select new
                {
                    r.ID,
                    r.Name
                }
            ).ToListAsync();

            var buttons = roles.Select(r => new InlineKeyboardButton[] { new InlineKeyboardButton(r.Name) { CallbackData = "role=" + r.ID } });

            var keyboard = new InlineKeyboardMarkup(buttons.Append(new InlineKeyboardButton[] { new InlineKeyboardButton("Добавить") { CallbackData = "create_role=" } }));

            await _bot.SendTextMessageAsync(_message.Chat.Id, "Выберите роль персонажа:", replyMarkup: keyboard);
        }
    }
}
