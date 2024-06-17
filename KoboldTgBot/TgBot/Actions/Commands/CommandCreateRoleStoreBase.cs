using KoboldTgBot.Database;
using KoboldTgBot.TgBot.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal abstract class CommandCreateRoleStoreBase : TgCommandBase
    {
        internal CommandCreateRoleStoreBase(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected async Task Store(Action<DbRole, string> setter, string info, StateCreateRole nextState)
        {
            var smCreateRole = _data as StateMachineCreateRole;

            setter(smCreateRole!.Role[_message.From!.Id], _message.Text!);

            smCreateRole.CreateState(nextState, _message.Chat.Id);

            var msg = await _bot.SendTextMessageAsync(_message.Chat.Id, info);

            smCreateRole.AddMessageToDelete(_message.Chat.Id, _message.MessageId);
            smCreateRole.AddMessageToDelete(msg.Chat.Id, msg.MessageId);
        }
    }
}
