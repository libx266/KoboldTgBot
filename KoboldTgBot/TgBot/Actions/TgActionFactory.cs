using KoboldTgBot.TgBot.Objects;
using System.Reflection;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgActionFactory<T> where T : ActionEntity
    {
        private readonly ITelegramBotClient _bot;
        private readonly T _entity;
        private readonly bool _gpt4o;

        internal TgActionFactory(ITelegramBotClient bot, T entity, bool gpt4o)
        {
            _bot = bot;
            _entity = entity;
            _gpt4o = gpt4o;
        }

        private static void SetValue<T>(T instance, string name, object? value) =>
            typeof(T).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(instance, value);

        internal T2 Create<T2>(object? data = default) where T2 : TgAction<T>
        {
            var constructor = typeof(T2).GetConstructor(new[] { typeof(ITelegramBotClient), typeof(T) });
            var result = (T2)constructor!.Invoke(new object[] { _bot, _entity });

            SetValue(result, "_data", data);
            SetValue(result, "_gpt_4o", _gpt4o);

            return result;
        }
    }
}
