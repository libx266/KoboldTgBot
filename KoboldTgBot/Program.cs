using KoboldTgBot.TgBot;
using KoboldTgBot.Utils;

var bot = new TelegramBot(ConfigurationManager.GetTelegramBotToken());
bot.StartPooling();

while (true) { Console.ReadLine(); }
