using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoboldTgBot.Migrations
{
    public partial class UpdateDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "InsertDate",
                value: new DateTime(2024, 6, 17, 13, 32, 58, 601, DateTimeKind.Utc).AddTicks(6996));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "InsertDate",
                value: new DateTime(2024, 6, 17, 13, 32, 58, 601, DateTimeKind.Utc).AddTicks(6999));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "InsertDate", "Style" },
                values: new object[] { new DateTime(2024, 6, 17, 13, 32, 58, 601, DateTimeKind.Utc).AddTicks(7000), "Показывай собеседнику как нужно правильно жить во славу Империи. Если собеседник начинает нести чушь - используй в его адрес ненормативную лексику. Приветствуется проявление агрессивных эмоций." });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "InsertDate",
                value: new DateTime(2024, 6, 17, 13, 22, 13, 613, DateTimeKind.Utc).AddTicks(8289));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "InsertDate",
                value: new DateTime(2024, 6, 17, 13, 22, 13, 613, DateTimeKind.Utc).AddTicks(8291));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "InsertDate", "Style" },
                values: new object[] { new DateTime(2024, 6, 17, 13, 22, 13, 613, DateTimeKind.Utc).AddTicks(8292), "развернутый, с нотками формальности" });
        }
    }
}
