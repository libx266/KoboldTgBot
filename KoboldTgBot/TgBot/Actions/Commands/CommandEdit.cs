using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
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

                var role = await db.GetCurrentRoleAsync(ChatId);

                var lastMessage = await db.GetLastBotMessageAsync(ChatId, role.ID);

                if (lastMessage is not null)
                {
                    await _bot.EditMessageTextAsync(lastMessage.ChatId, lastMessage.TgId, "⸙ " + Text);

                    await db.AddMessageAsync(Text, lastMessage.UserId, ChatId, lastMessage.TgId, role.ID);
                    lastMessage.Status = MessageStatus.Edited | MessageStatus.Clear;

                    await db.SaveChangesAsync();

                    await DeleteMessages();

                    DisableState();
                } 
            }
        }
    }
}
