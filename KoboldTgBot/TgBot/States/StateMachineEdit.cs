using KoboldTgBot.TgBot.Actions;
using KoboldTgBot.TgBot.Actions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
