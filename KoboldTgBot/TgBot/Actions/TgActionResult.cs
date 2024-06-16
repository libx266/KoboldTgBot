namespace KoboldTgBot.TgBot.Actions
{
    internal record TgActionResult(bool Status, Exception? Error = default, object? Data = default);
}
