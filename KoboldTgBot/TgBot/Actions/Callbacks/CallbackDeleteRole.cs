using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackDeleteRole : TgCallbackBase
    {
        public CallbackDeleteRole(ITelegramBotClient bot, CallbackQuery callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            int roleId = Int32.Parse(Data!);

            using var db = new DataContext();

            var role = await db.Roles.FirstAsync(r => r.ID == roleId);
            db.Roles.Remove(role);
            await db.SaveChangesAsync();

            await _bot.EditMessageTextAsync(_callback.Message!.Chat.Id, _callback.Message.MessageId, "Удалена роль:  " + role.Title);
        }
    }
}
