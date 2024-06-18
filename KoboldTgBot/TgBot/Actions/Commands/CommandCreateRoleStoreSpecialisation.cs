using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreSpecialisation : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreSpecialisation(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
            await Store((role, msg) => role.Specialisation = msg, "Какое отношение должно быть у вашего персонажа к собеседнику?", States.StateCreateRole.Relation);
    }
}
