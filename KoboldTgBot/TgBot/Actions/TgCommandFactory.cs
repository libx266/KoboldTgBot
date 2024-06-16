using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions
{
    internal sealed class TgCommandFactory
    {
        private readonly ITelegramBotClient _bot;
        private readonly Message _message;

        internal TgCommandFactory(ITelegramBotClient bot, Message message)
        {
            _bot = bot;
            _message = message;
        }

        internal T CreateComand<T>(object? data = default) where T : TgCommandBase
        {
            var constructor = typeof(T).GetConstructor(new[] { typeof(ITelegramBotClient), typeof(Message) });
            var result = (T)constructor!.Invoke(new object[] { _bot, _message });

            typeof(T).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(result, data);

            return result;
        }
    }
}
