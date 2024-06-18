using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreCharakter : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreCharakter(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
            await Store((role, msg) => role.Charakter = msg, "Чем увлекается ваш персонаж? Где он учится/работает?", States.StateCreateRole.Specialisation);
    }
}
