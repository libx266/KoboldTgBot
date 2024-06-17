using KoboldTgBot.TgBot.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackCreateRole : TgCallbackBase
    {
        public CallbackCreateRole(ITelegramBotClient bot, CallbackQuery callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            var smCreateRole = _data as StateMachineCreateRole;
            smCreateRole!.CreateState(StateCreateRole.Name, _callback.Message!.Chat.Id);

            await _bot.EditMessageTextAsync(_callback.Message.Chat.Id, _callback.Message.MessageId, "Введите название роли");

            smCreateRole.AddMessageToDelete(_callback.Message.Chat.Id, _callback.Message.MessageId);
        }
    }
}
