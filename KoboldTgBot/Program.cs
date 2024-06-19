using KoboldTgBot.TgBot;
using KoboldTgBot.Utils;

var bot = new TelegramBot(ConfigurationManager.GetTelegramBotToken());
bot.StartPooling();

while (true)
{
    Action? cmd = Console.ReadLine() switch
    {
        "clear" => () => Console.Clear(),
        _ => default
    };

    cmd?.Invoke();
}
