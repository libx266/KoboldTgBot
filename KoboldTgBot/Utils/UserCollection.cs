namespace KoboldTgBot.Utils
{
    internal sealed class UserCollection<T>
    {
        private readonly Dictionary<long, HashSet<T>> _store = new();

        internal void AddItem(long chatId, T item)
        {
            if (_store.ContainsKey(chatId))
            {
                _store[chatId].Add(item);
            }
            else
            {
                _store.Add(chatId, new HashSet<T> { item });
            }
        }

        internal HashSet<T> GetItems(long chatId) =>
            _store.TryGetValue(chatId, out var r) ? r : new HashSet<T>();

        internal void ClearItems(long chatId) =>
            _store.Remove(chatId);

    }
}
