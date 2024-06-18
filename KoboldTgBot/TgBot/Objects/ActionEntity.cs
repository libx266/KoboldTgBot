namespace KoboldTgBot.TgBot.Objects
{
    public abstract class ActionEntity
    {
        internal abstract long ChatId { get; }
        internal abstract long UserId { get; }
        internal abstract int MessageId { get; }
        internal abstract string Text { get; }
    }
}
