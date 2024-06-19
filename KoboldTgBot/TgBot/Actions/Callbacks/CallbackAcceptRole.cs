using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackAcceptRole : TgAction<CallbackHandler>
    {
        public const string Name = "accept_role";

        public CallbackAcceptRole(ITelegramBotClient bot, CallbackHandler callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            int roleId = Int32.TryParse(Entity.Data, out int r) ? r : 1;

            await db.AcceptRoleAsync(ChatId, roleId);
            await db.ClearContextAsync(ChatId);

            await db.SaveChangesAsync();

            await _bot.EditMessageTextAsync(ChatId, MessageId, "Применена роль:  " + await db.Roles.Where(r => r.ID == roleId).Select(r => r.Title).FirstAsync());
        }
    }
}
