using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;

namespace KoboldTgBot.Extensions.Database
{
    internal static class RoleHelper
    {
        internal static async Task<DbRole> GetRoleByIdAsync(this DataContext db, int roleId) =>
            await db.Roles.FirstAsync(r => r.ID == roleId);

        /// <summary>
        /// require save changes
        /// </summary>
        /// <param name="db"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        internal static async Task<string> DeleteRoleByIdAsync(this DataContext db, int roleId) =>
            db.Roles.Remove(await GetRoleByIdAsync(db, roleId)).Entity.Title;

        internal static async Task<List<RoleShortDto>> GetRoleShortListAsync(this DataContext db, long userId) => await
        (
            from r in db.Roles
            where r.UserId == userId ||
            r.UserId == -1
            select new RoleShortDto(r.ID, r.Title)
        ).ToListAsync();
    }
}
