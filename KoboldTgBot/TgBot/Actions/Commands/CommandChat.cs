using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
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

                var role = await db.GetCurrentRoleAsync(ChatId);

                if (_data == default)
                {
                    await db.AddMessageAsync(Text, UserId, ChatId, MessageId, role.ID);
                    await db.SaveChangesAsync();
                }

                var sendedMessage = await _bot.SendTextMessageAsync(ChatId, await GenerateAsync(db));

                await db.AddMessageAsync(sendedMessage.Text!, -1L, sendedMessage.Chat.Id, sendedMessage.MessageId, role.ID);
                await db.SaveChangesAsync();
            }
        }
    }
}
