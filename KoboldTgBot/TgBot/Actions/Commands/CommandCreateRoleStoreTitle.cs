using KoboldTgBot.TgBot.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreTitle : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreTitle(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() => 
            await Store((role, msg) => role.Title = msg, "Как зовут вашего персонажа?", StateCreateRole.Name);
    }
}
