using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackCreateRole : TgStatedAction<CallbackHandler, StateCreateRole, StateMachineCreateRole>
    {
        public const string Name = "create_role";

        public CallbackCreateRole(ITelegramBotClient bot, CallbackHandler callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            CreateState(StateCreateRole.Title);

            GetSmData(sm => sm.Role)[UserId] = new DbRole();

            await _bot.EditMessageTextAsync(ChatId, MessageId, "Введите название роли");

            await DeleteMessages(MessageId);

            AddMessageToDelete(MessageId);
        }
    }
}
