using KoboldTgBot.TgBot.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal class CommandEditPrepare : TgCommandBase
    {
        public CommandEditPrepare(ITelegramBotClient bot, Message message) : base(bot, message)
        { 
        }

        protected override async Task WorkAsync()
        {
            var smEdit = _data as StateMachineEdit;

            smEdit!.AddMessageToDelete(_message.Chat.Id, _message.MessageId);

            var msg = await _bot.SendTextMessageAsync(_message.Chat.Id, Properties.Resources.EditCommand);

            smEdit.AddMessageToDelete(_message.Chat.Id, msg.MessageId);

            smEdit.CreateState(StateEdit.Process, _message.Chat.Id);
        }
    }
}
