using KoboldTgBot.TgBot.Objects;
using System.Reflection;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgActionFactory<T> where T : ActionEntity
    {
        private readonly ITelegramBotClient _bot;
        private readonly T _entity;

        internal TgActionFactory(ITelegramBotClient bot, T entity)
        {
            _bot = bot;
            _entity = entity;
        }

        private static void SetValue<T2>(T2 instance, string name, object? value) =>
            typeof(T2).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(instance, value);

        internal T2 Create<T2>(object? data = default) where T2 : TgAction<T>
        {
            var constructor = typeof(T2).GetConstructor(new[] { typeof(ITelegramBotClient), typeof(T) });
            var result = (T2)constructor!.Invoke(new object[] { _bot, _entity });

            SetValue(result, "_data", data);

            return result;
        }
    }
}
