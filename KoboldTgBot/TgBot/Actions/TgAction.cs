using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgAction<T> where T : ActionEntity
    {
        protected object? _data = default;

        protected readonly ITelegramBotClient _bot;
        protected readonly T Entity;

        protected readonly bool _gpt4o;

        protected long ChatId => Entity.ChatId;
        protected long UserId => Entity.UserId;
        protected int MessageId => Entity.MessageId;
        protected string Text => Entity.Text;

        internal TgAction(ITelegramBotClient bot, T entity)
        {
            _bot = bot;
            Entity = entity;
        }

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
