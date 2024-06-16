using KoboldTgBot.Neuro;
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
    internal sealed class CommandRole : TgCommandBase
    {
        public CommandRole(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[] {new InlineKeyboardButton("Офицер по науке") { CallbackData = "role=" + NeuroCharacterRoleManager.Science} },
                new InlineKeyboardButton[] {new InlineKeyboardButton("Старожила аниме бесед") { CallbackData = "role=" + NeuroCharacterRoleManager.ChitChat } },
                new InlineKeyboardButton[] {new InlineKeyboardButton("Командир Императорской гвардии") { CallbackData = "role=" + NeuroCharacterRoleManager.Imperial } }
            });

            await _bot.SendTextMessageAsync(_message.Chat.Id, "Выберите роль персонажа:", replyMarkup: keyboard);
        }
    }
}
