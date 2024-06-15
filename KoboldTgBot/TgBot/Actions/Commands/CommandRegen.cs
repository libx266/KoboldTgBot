using KoboldTgBot.Database;
using KoboldTgBot.Neuro;
using KoboldTgBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandRegen : TgCommandBase
    {
        public CommandRegen(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            var lastMessage = db.Messages.Where(m => m.ChatId == _message.Chat.Id && m.InMemory).OrderByDescending(m => m.ID).FirstOrDefault();

            if (lastMessage is not null && lastMessage.SenderId == -1L)
            {
                lastMessage.InMemory = false;
                await db.SaveChangesAsync();

                var prompt = await db.ConstructPropmptAsync(_message.Chat.Id, _message.From);

                string answer = await GenerationApi.GenerateAsync(prompt, 1024, 8192);

                await _bot.EditMessageTextAsync(_message.Chat.Id, (int)lastMessage.TgId, answer);

                await db.Messages.AddAsync(new DbMessage
                {
                    ChatId = _message.Chat.Id,
                    SenderId = -1L,
                    Text = answer,
                    TgId = lastMessage.TgId
                });

                await db.SaveChangesAsync();

                await _bot.DeleteMessageAsync(_message.Chat.Id, _message.MessageId);
            }
        }
    }
}
