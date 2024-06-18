using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions
{
    internal sealed class TgCommandFactory : TgActionFactory<MessageHandler>
    {
        public TgCommandFactory(ITelegramBotClient bot, Message message) : base(bot, new MessageHandler(message))
        {
        }
    }
}
