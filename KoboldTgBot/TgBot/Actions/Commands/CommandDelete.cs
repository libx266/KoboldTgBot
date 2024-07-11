using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandDelete : TgAction<MessageHandler>
    {
        public const string Name = "/delete";

        public CommandDelete(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            var id = await db.DeleteLastMessage(ChatId);

            await _bot.DeleteMessageAsync(ChatId, MessageId);

            if (id != default)
            {
                await _bot.DeleteMessageAsync(ChatId, id.Value);
            }
        }
    }
}
