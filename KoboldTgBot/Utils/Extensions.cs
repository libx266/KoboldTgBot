using KoboldTgBot.TgBot.Actions;
using KoboldTgBot.TgBot.Objects;
using Newtonsoft.Json;
using System.Reflection;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.Utils
{
    internal static class Extensions
    {
        internal static void Log(this Exception ex) =>
            Task.Run(() => Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented)));


        private static IEnumerable<char> FilterEmojis(string text)
        {
            foreach(var c in text)
            {
                if
                (
                    (c >= 0x400 && c <= 0x491) || // Кириллица
                             c <= 0x7E         ||   // ASCII
                    (c >= 0xA1 && c <= 0xFF)      //дополнения к латинице
                )
                {
                    yield return c;
                }
            }
        }

        internal static string RemoveEmojis(string text) => 
            String.Join(default(string), FilterEmojis(text));

        internal static InlineKeyboardButton MakeInlineButton<T>(string text, object? callbackData = default) where T : TgAction<CallbackHandler>
        {
            var name = typeof(T).GetField("Name", BindingFlags.Public | BindingFlags.Static)!.GetValue(default)!.ToString();
            return new InlineKeyboardButton(text) { CallbackData = name + '=' + callbackData?.ToString() ?? "" };
        }
            
            
    }
}
