using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreGender : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreGender(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
            await Store((role, msg) => role.Gender = msg, "Опишите характер вашего персонажа.", States.StateCreateRole.Charakter);
    }
}
