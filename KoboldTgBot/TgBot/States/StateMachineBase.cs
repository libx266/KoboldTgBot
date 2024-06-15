using KoboldTgBot.TgBot.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.States
{
    internal abstract class StateMachineBase<T> where T : Enum
    {
        protected readonly Dictionary<long, T> CurrentStates = new();

        internal void CreateState(T state, long chatId) => CurrentStates[chatId] = state;

        internal void DisableState(long chatId) => CurrentStates.Remove(chatId);

        internal bool IsEnable(long chatId, out T state) => CurrentStates.TryGetValue(chatId, out state);

    }
}
