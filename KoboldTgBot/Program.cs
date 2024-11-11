using KoboldTgBot.TgBot;
using KoboldTgBot.Utils;

var bot = new TelegramBot(ConfigurationManager.TelegramBotToken);
bot.StartPooling();

Console.WriteLine("Bot started.");

Action? cmd = Console.ReadLine() switch
{
    "clear" => () => Console.Clear(),
    _ => default
};

while (true) { cmd?.Invoke(); }
