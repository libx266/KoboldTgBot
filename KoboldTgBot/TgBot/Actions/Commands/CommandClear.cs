using KoboldTgBot.Database;
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

            db.Messages.Where(m => m.ChatId == ChatId).ToList().ForEach(m => m.InMemory = false);

            await db.SaveChangesAsync();

            await _bot.SendTextMessageAsync(ChatId, "Контекст сброшен");
        }
    }
}
