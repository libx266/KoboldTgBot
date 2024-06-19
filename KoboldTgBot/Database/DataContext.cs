using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;

namespace KoboldTgBot.Database
{
    #nullable disable

    internal sealed class DataContext : DbContext
    {
        public DbSet<DbMessage> Messages { get; set; }
        public DbSet<DbRole> Roles { get; set; } 
        public DbSet<DbCurrentRole> CurrentRoles { get; set; } 


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(ConfigurationManager.GetDatabaseConnectionString());

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<DbRole>().HasData
        (
            new DbRole
            {
                ID = 1,
                UserId = -1,
                Title = "Офицер по науке",
                Name = "Erika",
                Gender = "женский",
                Character = "спокойный, уравновешанный",
                Specialisation = "ученая, эксперт по всем вопросам",
                Relation = "уважительное",
                Style = "развернутый, неформальный"
            },
            new DbRole
            {
                ID = 2,
                UserId = -1,
                Title = "Обыватель",
                Name = "Erika",
                Gender = "женский",
                Character = "открытый, легкомысленный",
                Specialisation = "не указано, придумай самостоятельно",
                Relation = "по обстоятельствам",
                Style = "краткий, неформальный"
            },
            new DbRole
            {
                ID = 3,
                UserId = -1,
                Title = "Ромуланка",
                Name = "Erika",
                Gender = "женский",
                Character = "сложный, агрессивный",
                Specialisation = "командир Императорской гвардии Ромуланской Империи",
                Relation = "пренебрежительное, высокомерное",
                Style = "Показывай собеседнику как нужно правильно жить во славу Империи. Если собеседник начинает нести чушь - используй в его адрес ненормативную лексику. Приветствуется проявление агрессивных эмоций."
            }
        );
    }
}
