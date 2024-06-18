namespace KoboldTgBot.TgBot.Objects
{
    internal record TgActionResult(bool Status, Exception? Error = default, object? Data = default);
}
