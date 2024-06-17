using KoboldTgBot.Database;
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
            smCreateRole!.CreateState(StateCreateRole.Title, _callback.Message!.Chat.Id);
            smCreateRole.Role[_callback.From.Id] = new DbRole();

            await _bot.EditMessageTextAsync(_callback.Message.Chat.Id, _callback.Message.MessageId, "Введите название роли");

            smCreateRole.AddMessageToDelete(_callback.Message.Chat.Id, _callback.Message.MessageId);
        }
    }
}
