using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandRegen : CommandWithGenerationBase
    {
        public const string Name = "/regen";

        public CommandRegen(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            var lastMessage = await db.Messages.Where(m => m.ChatId == ChatId && m.InMemory).OrderByDescending(m => m.ID).FirstOrDefaultAsync();

            if (lastMessage is not null && lastMessage.UserId == -1L)
            {
                lastMessage.InMemory = false;
                await db.SaveChangesAsync();

                string answer = await GenerateAsync(db);

                await _bot.EditMessageTextAsync(ChatId, lastMessage.TgId, answer);

                await db.Messages.AddAsync(new DbMessage
                {
                    ChatId = ChatId,
                    UserId = -1L,
                    Text = answer,
                    TgId = lastMessage.TgId
                });

                await db.SaveChangesAsync();

                await _bot.DeleteMessageAsync(ChatId, MessageId);
            }
        }
    }
}
