using KoboldTgBot.Database;
using KoboldTgBot.TgBot.Objects;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackDeleteRole : TgAction<CallbackHandler>
    {
        public const string Name = "delete_role";

        public CallbackDeleteRole(ITelegramBotClient bot, CallbackHandler callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            int roleId = Int32.Parse(Entity.Data);

            using var db = new DataContext();

            var role = await db.Roles.FirstAsync(r => r.ID == roleId);
            db.Roles.Remove(role);
            await db.SaveChangesAsync();

            await _bot.EditMessageTextAsync(ChatId, MessageId, "Удалена роль:  " + role.Title);
        }
    }
}
