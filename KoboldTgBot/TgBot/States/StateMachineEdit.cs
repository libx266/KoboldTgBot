namespace KoboldTgBot.TgBot.States
{
    internal class StateMachineEdit : StateMachineBase<StateEdit>
    {
        protected readonly Dictionary<long, List<int>> DeletingMessagesId = new();

        internal void AddMessageToDelete(long chatId, int messageId)
        {
            if (DeletingMessagesId.ContainsKey(chatId))
            {
                DeletingMessagesId[chatId].Add(messageId);
            }
            else
            {
                DeletingMessagesId.Add(chatId, new List<int> { messageId});
            }
        }

        internal List<int> GetMessagesToDelete(long chatId) => DeletingMessagesId[chatId];

    }
}
