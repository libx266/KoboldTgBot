﻿using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
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

            var role = await db.GetCurrentRoleAsync(ChatId);

            var lastMessage = await db.GetLastBotMessageAsync(ChatId, role.ID);

            if (lastMessage is not null)
            {
                lastMessage.Status = MessageStatus.Regenerated | MessageStatus.Clear;
                await db.SaveChangesAsync();

                string answer = await GenerateAsync(db);

                var msg = await _bot.SendTextMessageAsync(ChatId, answer);

                await db.AddMessageAsync(answer, -1L, ChatId, msg.MessageId, role.ID);

                await db.SaveChangesAsync();

                await _bot.DeleteMessageAsync(ChatId, MessageId);
                await _bot.DeleteMessageAsync(ChatId, lastMessage.TgId);
            }
        }
    }
}
