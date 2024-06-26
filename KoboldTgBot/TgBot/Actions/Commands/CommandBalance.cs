using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandBalance : TgAction<MessageHandler>
    {
        internal const string Gpt4o = "gpt-4o";
        internal const string LLama3 = "LLaMa-3 70B";

        public const string Name = "/balance";

        public CommandBalance(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            var cab = db.Cabinets.FirstOrDefault(c => c.UserId == UserId);

            if (cab == default)
            {
                cab = new DbCabinet { UserId = UserId };
                await db.Cabinets.AddAsync(cab);
            }

            await db.SaveChangesAsync();

            bool gpt4oEnable = !cab.IsGpt4o && cab.Balance > 0;

            string model = gpt4oEnable ? Gpt4o : LLama3;

            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton("Переключить на " + model) { CallbackData = String.Join('=', CallbackSelectModel.Name, model) });

            await _bot.SendTextMessageAsync(ChatId, "Ваш баланс:  " + cab.Balance, replyMarkup: keyboard);
        }
    }
}
