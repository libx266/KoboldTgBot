using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions
{
    internal sealed class TgCallbackFactory : TgActionFactory<CallbackHandler>
    {
        public TgCallbackFactory(ITelegramBotClient bot, CallbackQuery callback, bool gpt4o) : base(bot, new CallbackHandler(callback), gpt4o)
        {
        }
    }
}
