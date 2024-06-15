using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions
{
    internal class TgCommandFactory
    {
        protected readonly ITelegramBotClient _bot;
        protected readonly Message _message;

        internal TgCommandFactory(ITelegramBotClient bot, Message message)
        {
            _bot = bot;
            _message = message;
        }

        internal virtual T CreateComand<T>() where T : TgCommandBase
        {
            var constructor = typeof(T).GetConstructor(new[] { typeof(ITelegramBotClient), typeof(Message) });
            return (T)constructor.Invoke(new object[] { _bot, _message });
        }
    }
}
