using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoboldTgBot.Migrations
{
    public partial class UpdsteDefaultRoles2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "InsertDate", "Title" },
                values: new object[] { new DateTime(2024, 6, 20, 12, 27, 44, 413, DateTimeKind.Utc).AddTicks(9602), "Эксперт" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "InsertDate",
                value: new DateTime(2024, 6, 20, 12, 27, 44, 413, DateTimeKind.Utc).AddTicks(9604));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "InsertDate", "Specialisation", "Title" },
                values: new object[] { new DateTime(2024, 6, 20, 12, 27, 44, 413, DateTimeKind.Utc).AddTicks(9605), "командир Императорской гвардии Роccийской Империи", "Офицер" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "InsertDate", "Title" },
                values: new object[] { new DateTime(2024, 6, 19, 1, 49, 31, 236, DateTimeKind.Utc).AddTicks(6989), "Офицер по науке" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "InsertDate",
                value: new DateTime(2024, 6, 19, 1, 49, 31, 236, DateTimeKind.Utc).AddTicks(6992));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "InsertDate", "Specialisation", "Title" },
                values: new object[] { new DateTime(2024, 6, 19, 1, 49, 31, 236, DateTimeKind.Utc).AddTicks(6993), "командир Императорской гвардии Ромуланской Империи", "Командир Императорской гвардии" });
        }
    }
}
