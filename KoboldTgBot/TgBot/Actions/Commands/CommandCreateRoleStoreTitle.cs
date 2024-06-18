using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreTitle : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreTitle(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() => 
            await Store((role, msg) => role.Title = msg, "Как зовут вашего персонажа?", StateCreateRole.Name);
    }
}
