using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreName : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreName(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
            await Store((role, msg) => role.Name = msg, "Укажите пол вашего персонажа.", States.StateCreateRole.Gender);
    }
}
