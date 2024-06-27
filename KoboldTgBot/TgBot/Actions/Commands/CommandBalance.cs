using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandBalance : TgWithBalance<MessageHandler>
    {
        

        public const string Name = "/balance";

        public CommandBalance(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            var cab = await db.RegisterCabinetAsync(UserId);

            await db.SaveChangesAsync();

            await SendCabInfoAsync(db, cab);
        }
    }
}
