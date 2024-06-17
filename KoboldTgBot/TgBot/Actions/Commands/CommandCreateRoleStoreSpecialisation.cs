using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreSpecialisation : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreSpecialisation(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
            await Store((role, msg) => role.Specialisation = msg, "Какое отношение должно быть у вашего персонажа к собеседнику?", States.StateCreateRole.Relation);
    }
}
