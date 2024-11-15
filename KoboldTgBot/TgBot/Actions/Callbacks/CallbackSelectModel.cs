using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackSelectModel : TgWithBalance<CallbackHandler>
    {
        public const string Name = "select_model";

        public CallbackSelectModel(ITelegramBotClient bot, CallbackHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {

            using var db = new DataContext();

            var cab = await db.GetCabinetAsync(UserId);

            cab!.ModelType = Entity.Data == ConfigurationManager.ModelName ? default : await db.Models.Where(m => m.Name == Entity.Data).Select(m => m.ID).FirstOrDefaultAsync();

            await db.SaveChangesAsync();

            await _bot.DeleteMessageAsync(ChatId, MessageId);

            await _bot.SendTextMessageAsync(ChatId, "Выбрана модель:  " + Entity.Data);
        }
    }
}
