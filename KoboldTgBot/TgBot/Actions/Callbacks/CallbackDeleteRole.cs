using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackDeleteRole : TgStatedAction<CallbackHandler, StateCreateRole, StateMachineCreateRole>
    {
        public const string Name = "delete_role";

        public CallbackDeleteRole(ITelegramBotClient bot, CallbackHandler callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            int roleId = Int32.Parse(Entity.Data);

            using var db = new DataContext();

            string title = await db.DeleteRoleByIdAsync(roleId);
            await db.SaveChangesAsync();

            await _bot.EditMessageTextAsync(ChatId, MessageId, "Удалена роль:  " + title);

            await DeleteMessages(MessageId);
        }
    }
}
