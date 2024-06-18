using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandEdit : TgStatedAction<MessageHandler, StateEdit, StateMachineEdit>
    {
        public const string Name = "/edit";

        public CommandEdit(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            if (StateIsDisable())
            {
                AddMessageToDelete(MessageId);

                var msg = await _bot.SendTextMessageAsync(ChatId, "Введите текст сообщения");

                AddMessageToDelete(msg.MessageId);

                CreateState(StateEdit.Process);
            }
            else
            {
                AddMessageToDelete(MessageId);

                using var db = new DataContext();

                var lastMessage = await db.Messages.Where(m => m.ChatId == ChatId && m.InMemory && m.UserId == -1L).OrderByDescending(m => m.ID).FirstOrDefaultAsync();

                if (lastMessage is not null)
                {
                    await _bot.EditMessageTextAsync(lastMessage.ChatId, lastMessage.TgId, $"🇵🇱 {Text}");

                    await db.Messages.AddAsync(new DbMessage
                    {
                        ChatId = ChatId,
                        TgId = lastMessage.TgId,
                        Text = Text!,
                        UserId = lastMessage.UserId,
                        IsEdited = true
                    });

                    lastMessage.InMemory = false;

                    await db.SaveChangesAsync();

                    await DeleteMessages();
                    DisableState();
                } 
            }
        }
    }
}
