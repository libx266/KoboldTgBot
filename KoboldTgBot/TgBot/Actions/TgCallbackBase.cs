using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgCallbackBase : TgActionBase
    {
        protected readonly CallbackQuery _callback;

        internal TgCallbackBase(ITelegramBotClient bot, CallbackQuery callback) : base(bot) => _callback = callback;
    }
}
