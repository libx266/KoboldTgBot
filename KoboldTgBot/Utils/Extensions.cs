using KoboldTgBot.Database;
using KoboldTgBot.Neuro;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

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

        public static string RemoveEmojis(string text) => 
            String.Join("", FilterEmojis(text));
            
    }
}
