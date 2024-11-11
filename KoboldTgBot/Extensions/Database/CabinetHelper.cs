using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;

namespace KoboldTgBot.Extensions.Database
{
    internal static class CabinetHelper
    {
        internal static async Task<DbCabinet?> GetCabinetAsync(this DataContext db, long userId) =>
            await db.Cabinets.FirstOrDefaultAsync(c => c.UserId == userId);


        internal static async Task<bool> IsExternalModelEnable(this DataContext db, long userId)
        {
            var cab = await GetCabinetAsync(db, userId);
            return Convert.ToBoolean(cab?.ModelType ?? default);
        }

        /// <summary>
        /// require save changes
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal static async Task<DbCabinet> RegisterCabinetAsync(this DataContext db, long userId)
        {
            var cab = await GetCabinetAsync(db, userId);

            if (cab == default)
            {
                cab = new DbCabinet { UserId = userId };
                await db.Cabinets.AddAsync(cab);
            }

            return cab;
        }

        /// <summary>
        /// require save changes
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId"></param>
        /// <param name="promptTokens"></param>
        /// <param name="completionTokens"></param>
        /// <returns></returns>
        internal static async Task<decimal> UpdateCabinetBalanceAsync(this DataContext db, long userId, decimal promptTokens, decimal completionTokens)
        {
            var cab = await GetCabinetAsync(db, userId);
            var model = await db.Models.FirstOrDefaultAsync(m => m.ID == cab!.ModelType);

            cab!.Balance -= ((promptTokens / 1000m) * model.Prompt1kTokensCostRub + (completionTokens / 1000m) * model.Answer1kTokensCostRub);

            if (cab.Balance < 0)
            {
                cab.ModelType = default;
            }

            return cab.Balance;
        }

    }
}
