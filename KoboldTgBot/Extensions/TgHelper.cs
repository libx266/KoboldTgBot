using KoboldTgBot.TgBot.Actions;
using KoboldTgBot.TgBot.Objects;
using System.Reflection;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.Extensions
{
    internal static class TgHelper
    {
        internal static InlineKeyboardButton MakeInlineButton<T>(string text, object? callbackData = default) where T : TgAction<CallbackHandler>
        {
            var name = typeof(T).GetField("Name", BindingFlags.Public | BindingFlags.Static)!.GetValue(default)!.ToString();
            return new InlineKeyboardButton(text) { CallbackData = name + '=' + callbackData?.ToString() ?? "" };
        }
    }
}
