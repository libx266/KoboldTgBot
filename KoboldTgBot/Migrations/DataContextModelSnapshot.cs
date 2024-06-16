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

                    b.Property<bool>("IsEdited")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("SenderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("SenderId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(4096)
                        .HasColumnType("character varying(4096)");

                    b.Property<long>("TgId")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
