using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgCommandBase : TgActionBase
    {
        protected readonly Message _message;

        internal TgCommandBase(ITelegramBotClient bot, Message message) : base(bot) => _message = message;
    }
}
