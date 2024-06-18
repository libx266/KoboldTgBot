using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Objects
{
    internal sealed class CallbackHandler : ActionEntity
    {
        private readonly CallbackQuery _callback;

        internal CallbackHandler(CallbackQuery callback) =>
            _callback = callback;

        internal override long ChatId => _callback.Message!.Chat.Id;
        internal override long UserId => _callback.From.Id;
        internal override int MessageId => _callback.Message!.MessageId;
        internal override string Text => _callback.Message!.Text!;
        internal string Data => _callback.Data!.Split('=').Last();

        internal T Get<T>(Func<CallbackQuery, T> getInfo) => getInfo(_callback);

    }
}
