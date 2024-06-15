using KoboldTgBot.Database;
using KoboldTgBot.TgBot.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandEditProcess : CommandEditPrepare
    {
        public CommandEditProcess(ITelegramBotClient bot, Message message, StateMachineEdit smEdit) : base(bot, message, smEdit)
        {
        }

        protected override async Task WorkAsync()
        {
            _smEdit.AddMessageToDelete(_message.Chat.Id, _message.MessageId);

            using var db = new DataContext();

            var lastMessage = db.Messages.Where(m => m.ChatId == _message.Chat.Id && m.InMemory && m.SenderId == -1L).OrderByDescending(m => m.ID).FirstOrDefault();

            if (lastMessage is not null)
            {
                await _bot.EditMessageTextAsync(lastMessage.ChatId, (int)lastMessage.TgId, _message.Text + "\n\n(изменено пользователем)");

                await db.Messages.AddAsync(new DbMessage
                {
                    ChatId = _message.Chat.Id,
                    TgId = lastMessage.TgId,
                    Text = _message.Text,
                    SenderId = lastMessage.SenderId,
                    IsEdited = true
                });

                lastMessage.InMemory = false;

                await db.SaveChangesAsync();

                foreach(int m in _smEdit.GetMessagesToDelete(_message.Chat.Id))
                {
                    await _bot.DeleteMessageAsync(_message.Chat.Id, m);
                }

                _smEdit.DisableState(_message.Chat.Id);
            }
        }
    }
}
