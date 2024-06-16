using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Reflection;

namespace KoboldTgBot.TgBot.Actions
{
    internal sealed class TgCallbackFactory
    {
        private readonly ITelegramBotClient _bot;
        private readonly CallbackQuery _callback;

        internal TgCallbackFactory(ITelegramBotClient bot, CallbackQuery callback)
        {
            _bot = bot;
            _callback = callback;
        }

        internal T CreateCallback<T>(object? data = default) where T : TgCallbackBase
        {
            var constructor = typeof(T).GetConstructor(new[] { typeof(ITelegramBotClient), typeof(CallbackQuery) });
            var result = (T)constructor!.Invoke(new object[] { _bot, _callback });

            typeof(T).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(result, data);

            return result;
        }
    }
}
