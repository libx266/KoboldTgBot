using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandMore : TgAction<MessageHandler>
    {
        public const string Name = "/more";

        public CommandMore(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            var factory = new TgCommandFactory(_bot, Entity.Get(m => m), _gpt4o);
            var chat = factory.Create<CommandChat>(new object());
            await chat.ExecuteAsync();

            await _bot.DeleteMessageAsync(ChatId, MessageId);
        }
    }
}
