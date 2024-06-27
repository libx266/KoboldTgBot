using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;

namespace KoboldTgBot.Extensions.Database
{
    internal static class CabinetHelper
    {
        internal static async Task<DbCabinet?> GetCabinetAsync(this DataContext db, long userId) =>
            await db.Cabinets.FirstOrDefaultAsync(c => c.UserId == userId);

        internal static async Task<bool> IsGpt4oEnable(this DataContext db, long userId)
        {
            var cab = await GetCabinetAsync(db, userId);
            return (cab?.IsGpt4o ?? false) && (cab?.Balance ?? 0) > 0;
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
            cab!.Balance -= ((promptTokens / 1000m) * cab.PromptTokenPrice + (completionTokens / 1000m) * cab.CompletionTokenPrice);

            if (cab.Balance < 0)
            {
                cab.IsGpt4o = false;
            }

            return cab.Balance;
        }

    }
}
