using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Actions.Commands;
using KoboldTgBot.TgBot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackSelectModel : TgAction<CallbackHandler>
    {
        public const string Name = "select_model";

        public CallbackSelectModel(ITelegramBotClient bot, CallbackHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            bool gpt4o = Entity.Data == CommandBalance.Gpt4o;

            using var db = new DataContext();

            var cab = db.Cabinets.First(c => c.UserId == UserId);

            cab.IsGpt4o = gpt4o;

            await db.SaveChangesAsync();

            await _bot.SendTextMessageAsync(ChatId, "Переключено на" + Entity.Data);
        }
    }
}
