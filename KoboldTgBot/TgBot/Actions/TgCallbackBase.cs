using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgCallbackBase : TgActionBase
    {
        protected readonly CallbackQuery _callback;

        internal TgCallbackBase(ITelegramBotClient bot, CallbackQuery callback) : base(bot) => _callback = callback;

        protected string? Data => _callback.Data?.Split('=').LastOrDefault();
    }
}
