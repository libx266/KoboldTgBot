using Newtonsoft.Json;

namespace KoboldTgBot.Utils
{
    #nullable disable

    public static class ConfigurationManager
    {
        private class Params
        {
            public string DatabaseConnectionString { get; set; }
            public string NeuroApiEndpoint { get; set; }
            public string TelegramBotToken { get; set; }
            public string ProxyAPIToken { get; set; }
            public string Gpt4oSecret { get; set; }
        }

        private static readonly Params _params = JsonConvert.DeserializeObject<Params>(File.ReadAllText("config.json"));

        public static string DatabaseConnectionString => _params.DatabaseConnectionString;
        public static string NeuroApiEndpoint => _params.NeuroApiEndpoint;
        public static string TelegramBotToken => _params.TelegramBotToken;
        public static string ProxyAPIToken => _params.ProxyAPIToken;
        public static string Gpt4oSecret => _params.Gpt4oSecret;
    }
}
