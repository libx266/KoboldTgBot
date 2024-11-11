using KoboldTgBot.Database;
using KoboldTgBot.Utils;
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
            var messages = await db.Messages.Where(m => m.ChatId == chatId && m.RoleId == roleId && m.Status == MessageStatus.Actual).ToListAsync();
            messages.ForEach(m => m.Status = MessageStatus.Clear);
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
        internal static async Task AddMessageAsync(this DataContext db, string text, long userId, long chatId, int messageId, int roleId) => await db.Messages.AddAsync(new DbMessage
        {
            Text = text,
            UserId = userId,
            ChatId = chatId,
            TgId = messageId,
            RoleId = roleId,
        });

        internal static async Task<DbMessage?> GetLastBotMessageAsync(this DataContext db, long chatId, int roleId) =>
            await db.Messages.Where(m => m.ChatId == chatId && m.Status == MessageStatus.Actual && m.UserId == -1L && m.RoleId == roleId).OrderByDescending(m => m.ID).FirstOrDefaultAsync();

        internal static async Task<List<MessageShortDto>> GetMessagesShortFilteredListAsync(this DataContext db, long chatId, int roleId, long userId)
        {
            int take = await db.IsExternalModelEnable(userId) ? Int32.MaxValue : ConfigurationManager.MaxContextLength / Convert.ToInt32(50f / ConfigurationManager.AverageSymbolsPerToken);

            var messages = await db.Messages.Where(m => m.ChatId == chatId && m.Status == MessageStatus.Actual && m.RoleId == roleId).OrderByDescending(m => m.ID).Take(take).ToListAsync();

            return
            (
                from m in messages
                group m by m.TgId into mg
                let m2 = mg.MaxBy(m => m.ID)
                orderby m2.ID descending
                select new MessageShortDto(m2.Text, m2.UserId)
            ).ToList();
        }

        internal static async Task<int?> DeleteLastMessage(this DataContext db, long chatId)
        {
            var msg = await db.Messages.OrderByDescending(m => m.ID).FirstOrDefaultAsync(m => m.ChatId == chatId && m.Status == MessageStatus.Actual);
            if (msg != default)
            {
                msg.Status = MessageStatus.Deleted | MessageStatus.Clear;
                await db.SaveChangesAsync();
                return msg.TgId;
            }
            return default;
        }
    }
}
