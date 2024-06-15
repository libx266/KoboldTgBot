using KoboldTgBot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandClear : TgCommandBase
    {
        public CommandClear(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            db.Messages.Where(m => m.ChatId == _message.Chat.Id).ToList().ForEach(m => m.InMemory = false);

            await db.SaveChangesAsync();

            await _bot.SendTextMessageAsync(_message.Chat.Id, Properties.Resources.ClearMessage);
        }
    }
}
