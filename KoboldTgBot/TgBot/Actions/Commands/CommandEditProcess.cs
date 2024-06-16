using KoboldTgBot.Database;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandEditProcess : TgCommandBase
    {
        public CommandEditProcess(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            var smEdit = _data as StateMachineEdit;

            smEdit.AddMessageToDelete(_message.Chat.Id, _message.MessageId);

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

                foreach(int m in smEdit.GetMessagesToDelete(_message.Chat.Id))
                {
                    await _bot.DeleteMessageAsync(_message.Chat.Id, m);
                }

                smEdit.DisableState(_message.Chat.Id);
            }
        }
    }
}
