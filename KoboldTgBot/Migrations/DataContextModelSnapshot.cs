﻿// <auto-generated />
using KoboldTgBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace KoboldTgBot.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("KoboldTgBot.Database.DbCurrentRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("CurrentRoles");
                });

            modelBuilder.Entity("KoboldTgBot.Database.DbMessage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<bool>("InMemory")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsEdited")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(4096)
                        .HasColumnType("character varying(4096)");

                    b.Property<int>("TgId")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("KoboldTgBot.Database.DbRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Charakter")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Relation")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Specialisation")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Style")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Charakter = "спокойный, уравновешанный",
                            Gender = "женский",
                            InsertDate = new DateTime(2024, 6, 17, 19, 10, 8, 25, DateTimeKind.Utc).AddTicks(7739),
                            Name = "Erika",
                            Relation = "уважительное",
                            Specialisation = "ученая, эксперт по всем вопросам",
                            Style = "развернутый, неформальный",
                            Title = "Офицер по науке",
                            UserId = -1L
                        },
                        new
                        {
                            ID = 2,
                            Charakter = "открытый, легкомысленный",
                            Gender = "женский",
                            InsertDate = new DateTime(2024, 6, 17, 19, 10, 8, 25, DateTimeKind.Utc).AddTicks(7742),
                            Name = "Erika",
                            Relation = "по обстоятельствам",
                            Specialisation = "не указано, придумай самостоятельно",
                            Style = "краткий, неформальный",
                            Title = "Обыватель",
                            UserId = -1L
                        },
                        new
                        {
                            ID = 3,
                            Charakter = "сложный, агрессивный",
                            Gender = "женский",
                            InsertDate = new DateTime(2024, 6, 17, 19, 10, 8, 25, DateTimeKind.Utc).AddTicks(7743),
                            Name = "Erika",
                            Relation = "пренебрежительное, высокомерное",
                            Specialisation = "командир Императорской гвардии Ромуланской Империи",
                            Style = "Показывай собеседнику как нужно правильно жить во славу Империи. Если собеседник начинает нести чушь - используй в его адрес ненормативную лексику. Приветствуется проявление агрессивных эмоций.",
                            Title = "Командир Императорской гвардии",
                            UserId = -1L
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
