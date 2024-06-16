using Newtonsoft.Json;

namespace KoboldTgBot.Utils
{
    public static class ConfigurationManager
    {
        private class Params
        {
            public string DatabaseConnectionString { get; set; } = null!;

            public string NeuroApiEndpoint { get; set; } = null!;

            public string TelegramBotToken { get; set; } = null!;
        }

        private static readonly Params? _params = JsonConvert.DeserializeObject<Params>(File.ReadAllText("config.json"));

        public static string GetDatabaseConnectionString() => _params!.DatabaseConnectionString;

        public static string GetNeuroApiEndpoint() => _params!.NeuroApiEndpoint;

        public static string GetTelegramBotToken() => _params!.TelegramBotToken;
    }
}
