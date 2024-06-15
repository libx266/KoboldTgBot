using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
