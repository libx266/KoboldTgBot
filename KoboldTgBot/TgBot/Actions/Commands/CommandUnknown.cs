using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandUnknown : TgAction<MessageHandler>
    {
        public CommandUnknown(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync() =>
           await _bot.SendTextMessageAsync(ChatId, "Данная команда не зарегистрирована");
    }
}
