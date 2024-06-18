using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal abstract class CommandCreateRoleStoreBase : TgStatedAction<MessageHandler, StateCreateRole, StateMachineCreateRole>
    {
        public CommandCreateRoleStoreBase(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected async Task Store(Action<DbRole, string> setter, string info, StateCreateRole nextState)
        {
            setter(GetSmData(sm => sm.Role)[UserId], Text);

            CreateState(nextState);

            var msg = await _bot.SendTextMessageAsync(ChatId, info);

            AddMessageToDelete(MessageId);
            AddMessageToDelete(msg.MessageId);
        }
    }
}
