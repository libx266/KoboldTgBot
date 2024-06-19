using Newtonsoft.Json;

namespace KoboldTgBot.Extensions.Utils
{
    internal static class ExceptionHelper
    {
        internal static void Log(this Exception ex) =>
            Task.Run(() => Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented)));
    }
}
