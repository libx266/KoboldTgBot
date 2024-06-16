using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandStart : TgCommandBase
    {
        public CommandStart(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[]
            {
                new KeyboardButton("/clear"),
                new KeyboardButton("/regen"),
                new KeyboardButton("/edit")
              
            });

            keyboard.ResizeKeyboard = true;

            await _bot.SendTextMessageAsync(_message.Chat.Id, Properties.Resources.StartMessage, replyMarkup: keyboard);
        }
            
    }
}
