using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreRelation : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreRelation(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
            await Store((role, msg) => role.Relation = msg, "Какого стиля ведения диалога персонаж должен приедерживаться?", States.StateCreateRole.Style);
    }
}
