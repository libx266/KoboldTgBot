using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreStyle : TgStatedAction<MessageHandler, StateCreateRole, StateMachineCreateRole>
    {
        public CommandCreateRoleStoreStyle(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            var role = GetSmData(sm => sm.Role)[UserId];
            DisableState();

            GetSmData(sm => sm.Role).Remove(UserId);

            using var db = new DataContext();

            role.UserId = UserId;
            role.Style = Text;
            role.InsertDate = DateTime.UtcNow;

            await db.Roles.AddAsync(role);
            await db.SaveChangesAsync();

            AddMessageToDelete(MessageId);

            await DeleteMessages();

            await _bot.SendTextMessageAsync(ChatId, "Создана роль:  " + role.Title);
        }
    }
}
