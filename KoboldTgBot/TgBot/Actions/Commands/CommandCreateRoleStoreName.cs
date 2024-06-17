using KoboldTgBot.TgBot.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreName : TgCommandBase
    {
        public CommandCreateRoleStoreName(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            var msg = await _bot.SendTextMessageAsync(_message.Chat.Id, "Введите описание роли");

            var smCreateRole = _data as StateMachineCreateRole;

            smCreateRole!.Name[_message.From!.Id] = _message.Text!;
            smCreateRole.CreateState(StateCreateRole.Description, _message.Chat.Id);
            smCreateRole.AddMessageToDelete(_message.Chat.Id, _message.MessageId);
            smCreateRole.AddMessageToDelete(msg.Chat.Id, msg.MessageId);
        }
    }
}
