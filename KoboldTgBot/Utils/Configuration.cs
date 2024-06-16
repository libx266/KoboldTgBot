using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Utils
{
    public static class ConfigurationManager
    {
        private class Params
        {
            public string DatabaseConnectionString { get; set; }

            public string NeuroApiEndpoint { get; set; }

            public string TelegramBotToken { get; set; }
        }

        private static readonly Params _params = JsonConvert.DeserializeObject<Params>(File.ReadAllText("config.json"));

        public static string GetDatabaseConnectionString() => _params.DatabaseConnectionString;

        public static string GetNeuroApiEndpoint() => _params.NeuroApiEndpoint;

        public static string GetTelegramBotToken() => _params.TelegramBotToken;
    }
}
