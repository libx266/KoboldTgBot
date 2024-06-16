using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgActionBase
    {
        protected object? _data = default;

        protected readonly ITelegramBotClient _bot;

        internal TgActionBase(ITelegramBotClient bot) => _bot = bot;

        protected abstract Task WorkAsync();

        internal async Task<TgActionResult> ExecuteAsync()
        {
            try
            {
                await WorkAsync();
                return new TgActionResult(true, default, _data);
            }
            catch (Exception ex)
            {
                return new TgActionResult(false, ex, _data);
            }
        }
    }
}
