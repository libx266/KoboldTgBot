using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackSelectModel : TgWithBalance<CallbackHandler>
    {
        public const string Name = "select_model";

        internal const string Gpt4o = "gpt-4o";
        internal const string LLama3 = "LLaMa-3 70B";

        public CallbackSelectModel(ITelegramBotClient bot, CallbackHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            bool gpt4o = Entity.Data == Gpt4o;

            using var db = new DataContext();

            var cab = await db.GetCabinetAsync(UserId);

            cab!.IsGpt4o = gpt4o;

            await db.SaveChangesAsync();

            await _bot.SendTextMessageAsync(ChatId, "Переключено на " + Entity.Data);

            await SendCabInfoAsync(db, cab, true);
           
        }
    }
}
