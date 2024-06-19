using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;

namespace KoboldTgBot.Extensions.Database
{
    internal static class CurrentRoleHelper
    {
        /// <summary>
        /// require save changes
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        internal static async Task AcceptRoleAsync(this DataContext db, long chatId, int roleId)
        {
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
        }

        internal static async Task<DbRole> GetCurrentRoleAsync(this DataContext db, long chatId) => await
        (
            from cr in db.CurrentRoles.Where(cr => cr.ChatId == chatId).DefaultIfEmpty()
            from r in db.Roles
            where r.ID == (cr == default ? 1 : cr.RoleId)
            select r
        ).FirstAsync();
    }
}
