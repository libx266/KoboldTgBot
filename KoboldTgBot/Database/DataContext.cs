using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;

namespace KoboldTgBot.Database
{
    internal sealed class DataContext : DbContext
    {
        public DbSet<DbMessage> Messages { get; set; } = null!;
        public DbSet<DbRole> Roles { get; set; } = null!;
        public DbSet<DbCurrentRole> CurrentRoles { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(ConfigurationManager.GetDatabaseConnectionString());

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<DbRole>().HasData
        (
            new DbRole
            {
                ID = 1,
                UserId = -1,
                Name = "Офицер по науке",
                Description = Properties.Resources.NeuroCharacterSciencePrompt
            },
            new DbRole
            {
                ID = 2,
                UserId = -1,
                Name = "Обыватель",
                Description = Properties.Resources.NeuroCharacterChitChatPrompt
            },
            new DbRole
            {
                ID = 3,
                UserId = -1,
                Name = "Командир Императорской гвардии",
                Description = Properties.Resources.NeuroCharacterImperialPrompt
            }
        );
    }
}
