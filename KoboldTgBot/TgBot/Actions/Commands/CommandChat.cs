using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandChat : CommandWithGenerationBase
    {
        public CommandChat(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                using var db = new DataContext();

                if (_data == default)
                {
                    await db.Messages.AddAsync(new DbMessage
                    {
                        Text = Text,
                        UserId = UserId,
                        ChatId = ChatId,
                        TgId = MessageId
                    });

                    await db.SaveChangesAsync();
                }

                var sendedMessage = await _bot.SendTextMessageAsync(ChatId, await GenerateAsync(db));

                await db.Messages.AddAsync(new DbMessage
                {
                    Text = sendedMessage.Text!,
                    UserId = -1L,
                    ChatId = sendedMessage.Chat.Id,
                    TgId = sendedMessage.MessageId
                });

                await db.SaveChangesAsync();
            }
        }
    }
}
