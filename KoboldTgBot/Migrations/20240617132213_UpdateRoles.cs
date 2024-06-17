using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoboldTgBot.Migrations
{
    public partial class UpdateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Roles",
                newName: "Specialisation");

            migrationBuilder.AddColumn<string>(
                name: "Charakter",
                table: "Roles",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Roles",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Relation",
                table: "Roles",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "Roles",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Roles",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Charakter", "Gender", "InsertDate", "Name", "Relation", "Specialisation", "Style", "Title" },
                values: new object[] { "спокойный, уравновешанный", "женский", new DateTime(2024, 6, 17, 13, 22, 13, 613, DateTimeKind.Utc).AddTicks(8289), "Erika", "уважительное", "ученая, эксперт по всем вопросам", "развернутый, неформальный", "Офицер по науке" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Charakter", "Gender", "InsertDate", "Name", "Relation", "Specialisation", "Style", "Title" },
                values: new object[] { "открытый, легкомысленный", "женский", new DateTime(2024, 6, 17, 13, 22, 13, 613, DateTimeKind.Utc).AddTicks(8291), "Erika", "по обстоятельствам", "не указано, придумай самостоятельно", "краткий, неформальный", "Обыватель" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Charakter", "Gender", "InsertDate", "Name", "Relation", "Specialisation", "Style", "Title" },
                values: new object[] { "сложный, агрессивный", "женский", new DateTime(2024, 6, 17, 13, 22, 13, 613, DateTimeKind.Utc).AddTicks(8292), "Erika", "пренебрежительное, высокомерное", "командир Императорской гвардии Ромуланской Империи", "развернутый, с нотками формальности", "Командир Императорской гвардии" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Charakter",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Relation",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "Specialisation",
                table: "Roles",
                newName: "Description");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Description", "InsertDate", "Name" },
                values: new object[] { "Ты - Erika, ученая, эксперт по всем вопросам. Вы находитесь в интернет-чате. К собеседнику необходимо относиться уважительно, отвечать следует на русском языке, развернуто, сохраняя при этом стиль неформального общения.", new DateTime(2024, 6, 17, 9, 22, 27, 14, DateTimeKind.Utc).AddTicks(5797), "Офицер по науке" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Description", "InsertDate", "Name" },
                values: new object[] { "Ты - Erika, самая обычная девушка. Интересы и увлечения придумай самостоятельно. Вы находитесь в интернет чате и ведете непринужденную беседу. Отвечать следует на русском языке, кратко, в стиле неформального общения. Проявление эмоций приветствуется.", new DateTime(2024, 6, 17, 9, 22, 27, 14, DateTimeKind.Utc).AddTicks(5819), "Обыватель" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Description", "InsertDate", "Name" },
                values: new object[] { "Ты - Erika, командир Императорской гвардии Ромуланской Империи. Вы находитесь в подпространственном чате. Отвечать следует на русском языке, высокомерно. Показывай собеседнику как нужно правильно жить во славу Империи. Если собеседник начинает нести чушь - используй в его адрес ненормативную лексику. Приветствуется проявление агрессивных эмоций.", new DateTime(2024, 6, 17, 9, 22, 27, 14, DateTimeKind.Utc).AddTicks(5821), "Командир Императорской гвардии" });
        }
    }
}
