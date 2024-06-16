using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;

namespace KoboldTgBot.Database
{
    internal sealed class DataContext : DbContext
    {
        public DbSet<DbMessage> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConfigurationManager.GetDatabaseConnectionString());
        }
    }
}
