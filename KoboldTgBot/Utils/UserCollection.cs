namespace KoboldTgBot.Utils
{
    internal sealed class UserCollection<T>
    {
        private readonly Dictionary<long, List<T>> _store = new();

        internal void AddItem(long chatId, T item)
        {
            if (_store.ContainsKey(chatId))
            {
                _store[chatId].Add(item);
            }
            else
            {
                _store.Add(chatId, new List<T> { item });
            }
        }

        internal List<T> GetItems(long chatId) =>
            _store.TryGetValue(chatId, out var r) ? r : new List<T>();

        internal void ClearItems(long chatId) =>
            _store.Remove(chatId);

    }
}
