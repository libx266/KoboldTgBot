using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Objects
{
    internal sealed class MessageHandler : ActionEntity
    {
        private readonly Message _message;

        internal MessageHandler(Message message) =>
            _message = message;

        internal override long ChatId => _message.Chat.Id;
        internal override long UserId => _message.From!.Id;
        internal override int MessageId => _message.MessageId;
        internal override string Text => _message.Text!;

        internal T Get<T>(Func<Message, T> getter) =>
            getter(_message);

    }
}
