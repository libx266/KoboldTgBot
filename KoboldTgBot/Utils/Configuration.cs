using Newtonsoft.Json;

namespace KoboldTgBot.Utils
{
    #nullable disable

    public static class ConfigurationManager
    {
        private const string _path = "config.json";

        private class Params
        {
            public string DatabaseConnectionString { get; set; }
            public string NeuroApiEndpoint { get; set; }
            public string TelegramBotToken { get; set; }
            public string ProxyAPIToken { get; set; }
            public string PromptTemplate { get; set; }
            public float Temperature { get; set; }
            public float RepetitionPenalty { get; set; }
            public float TopPSampling { get; set; }
            public int MaxGenerationLength { get; set; }
            public int MaxContextLength { get; set; }
            public float AverageSymbolsPerToken { get; set; }
            public string ModelName { get; set; } 

        }

        private static Params _params => JsonConvert.DeserializeObject<Params>(File.ReadAllText("config.json"));

        public static string DatabaseConnectionString => _params.DatabaseConnectionString;
        public static string NeuroApiEndpoint => _params.NeuroApiEndpoint;
        public static string TelegramBotToken => _params.TelegramBotToken;
        public static string ProxyAPIToken => _params.ProxyAPIToken;
        public static string PromptTemplate => _params.PromptTemplate;
        public static float Temperature => _params.Temperature;
        public static float RepetitionPenalty => _params.RepetitionPenalty;
        public static float TopPSampling => _params.TopPSampling;
        public static int MaxGenerationLength => _params.MaxGenerationLength;
        public static int MaxContextLength => _params.MaxContextLength;
        public static float AverageSymbolsPerToken => _params.AverageSymbolsPerToken;
        public static string ModelName => _params.ModelName;
    }
}
