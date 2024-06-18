using KoboldTgBot.Extensions;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgStatedAction<T1, T2, T3> : TgAction<T1>
        where T1: ActionEntity 
        where T2 : Enum 
        where T3 : StateMachineBase<T2>
    {
        internal TgStatedAction(ITelegramBotClient bot, T1 entity) : base(bot, entity)
        {
        }

        private T3 GetStateMachine() => 
            (_data as T3)!;


        protected bool StateIsDisable() => 
            !GetStateMachine().IsEnable(ChatId, out var _);

        protected bool StateIsEnable(out T2 state) =>
            GetStateMachine().IsEnable(ChatId, out state);

        protected void CreateState(T2 state) =>
            GetStateMachine().CreateState(ChatId, state);

        protected void DisableState() =>
            GetStateMachine().DisableState(ChatId);

        protected T GetSmData<T>(Func<T3, T> getter) =>
            getter(GetStateMachine());

        
        protected void AddMessageToDelete(int messageId) =>
            GetStateMachine().MessagesToDelete.AddItem(ChatId, messageId);

        protected async Task DeleteMessages()
        {
            foreach (int m in GetStateMachine().MessagesToDelete.GetItems(ChatId))
            {
                try
                {
                    await _bot.DeleteMessageAsync(ChatId, m);
                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            }

            GetStateMachine().MessagesToDelete.ClearItems(ChatId);
        }
    }
}
