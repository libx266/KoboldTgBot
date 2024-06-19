using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandClear : TgAction<MessageHandler>
    {
        public const string Name = "/clear";

        public CommandClear(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            await db.ClearContextAsync(ChatId);
            await db.SaveChangesAsync();

            await _bot.SendTextMessageAsync(ChatId, "Контекст сброшен");
        }
    }
}
