namespace KoboldTgBot
{
    internal record PromptDto(string Prompt, string BotName, string UserName);
    internal record TgActionResult(bool Status, Exception? Error = default, object? Data = default);
    internal record RoleShortDto(int ID, string Title);
    internal record MessageShortDto(string Text, long Sender);

}
