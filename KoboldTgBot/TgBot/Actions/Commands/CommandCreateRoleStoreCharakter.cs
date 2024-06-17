using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreCharakter : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreCharakter(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
            await Store((role, msg) => role.Charakter = msg, "Чем увлекается ваш персонаж? Где он учится/работает?", States.StateCreateRole.Specialisation);
    }
}
