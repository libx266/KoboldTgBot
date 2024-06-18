using KoboldTgBot.Utils;

namespace KoboldTgBot.TgBot.States
{
    internal abstract class StateMachineBase<T> where T : Enum
    {
        private readonly Dictionary<long, T> CurrentStates = new();

        internal readonly UserCollection<int> MessagesToDelete = new();

        internal void CreateState(long chatId, T state) => CurrentStates[chatId] = state;

        internal void DisableState(long chatId) => CurrentStates.Remove(chatId);

        internal bool IsEnable(long chatId, out T state) => CurrentStates.TryGetValue(chatId, out state!);
    }
}
