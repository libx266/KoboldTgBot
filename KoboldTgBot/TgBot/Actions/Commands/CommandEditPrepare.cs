using KoboldTgBot.TgBot.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal class CommandEditPrepare : TgCommandBase
    {
        protected readonly StateMachineEdit _smEdit;
        public CommandEditPrepare(ITelegramBotClient bot, Message message, StateMachineEdit smEdit) : base(bot, message) =>
            _smEdit = smEdit;

        protected override async Task WorkAsync()
        {
            _smEdit.AddMessageToDelete(_message.Chat.Id, _message.MessageId);

            var msg = await _bot.SendTextMessageAsync(_message.Chat.Id, Properties.Resources.EditCommand);

            _smEdit.AddMessageToDelete(_message.Chat.Id, msg.MessageId);

            _smEdit.CreateState(StateEdit.Process, _message.Chat.Id);
        }
    }
}
