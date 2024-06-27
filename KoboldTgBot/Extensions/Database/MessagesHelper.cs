using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;

namespace KoboldTgBot.Extensions.Database
{
    internal static class MessagesHelper
    {
        /// <summary>
        /// require save changes
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        internal static async Task ClearContextAsync(this DataContext db, long chatId, int roleId)
        {
            var messages = await db.Messages.Where(m => m.ChatId == chatId && m.RoleId == roleId && m.InMemory).ToListAsync();
            messages.ForEach(m => m.InMemory = false);
        }

        /// <summary>
        /// require save changes
        /// </summary>
        /// <param name="db"></param>
        /// <param name="text"></param>
        /// <param name="userId"></param>
        /// <param name="chatId"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        internal static async Task AddMessageAsync(this DataContext db, string text, long userId, long chatId, int messageId, int roleId, bool isEdited = false) => await db.Messages.AddAsync(new DbMessage
        {
            Text = text,
            UserId = userId,
            ChatId = chatId,
            TgId = messageId,
            RoleId = roleId,
            IsEdited = isEdited
        });

        internal static async Task<DbMessage?> GetLastBotMessageAsync(this DataContext db, long chatId, int roleId) =>
            await db.Messages.Where(m => m.ChatId == chatId && m.InMemory && m.UserId == -1L && m.RoleId == roleId).OrderByDescending(m => m.ID).FirstOrDefaultAsync();

        internal static async Task<List<MessageShortDto>> GetMessagesShortFilteredListAsync(this DataContext db, long chatId, int roleId)
        {
            var messages = await db.Messages.Where(m => m.ChatId == chatId && m.InMemory && m.RoleId == roleId).OrderByDescending(m => m.ID).Take(byte.MaxValue).ToListAsync();

            return
            (
                from m in messages
                group m by m.TgId into mg
                let m2 = mg.MaxBy(m => m.ID)
                orderby m2.ID descending
                select new MessageShortDto(m2.Text, m2.UserId)
            ).ToList();
        }
    }
}
