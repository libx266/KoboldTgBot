using KoboldTgBot.Database;
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

            long chatId = ChatId;
            int roleId = Int32.TryParse(Entity.Data, out int r) ? r : 1;

            var currentRole = await db.CurrentRoles.FirstOrDefaultAsync(cr => cr.ChatId == chatId);
            if (currentRole is null)
            {
                await db.CurrentRoles.AddAsync(new DbCurrentRole
                {
                    ChatId = chatId,
                    RoleId = roleId
                });
            }
            else
            {
                currentRole.RoleId = roleId;
                currentRole.InsertDate = DateTime.UtcNow;
            }

            db.Messages.Where(m => m.ChatId == ChatId).ToList().ForEach(m => m.InMemory = false);

            await db.SaveChangesAsync();

            await _bot.EditMessageTextAsync(ChatId, MessageId, "Применена роль:  " + await db.Roles.Where(r => r.ID == roleId).Select(r => r.Title).FirstAsync());
        }
    }
}
