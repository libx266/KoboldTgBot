namespace KoboldTgBot.TgBot.States
{
    internal abstract class StateMachineBase<T> where T : Enum
    {
        private readonly Dictionary<long, T> CurrentStates = new();

        private readonly Dictionary<long, List<int>> MessagesToDelete = new();

        internal void AddMessageToDelete(long chatId, int messageId)
        {
            if (MessagesToDelete.ContainsKey(chatId))
            {
                MessagesToDelete[chatId].Add(messageId);
            }
            else
            {
                MessagesToDelete.Add(chatId, new List<int> { messageId });
            }
        }

        internal List<int> GetMessagesToDelete(long chatId) => MessagesToDelete[chatId];

        internal void ClearMessagesToDelete(long chatId) => MessagesToDelete.Remove(chatId);

        internal void CreateState(T state, long chatId) => CurrentStates[chatId] = state;

        internal void DisableState(long chatId) => CurrentStates.Remove(chatId);

        internal bool IsEnable(long chatId, out T state) => CurrentStates.TryGetValue(chatId, out state!);
    }
}
