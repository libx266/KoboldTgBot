using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoboldTgBot.Migrations
{
    public partial class UpdateRoleConstrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Style",
                table: "Roles",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "Relation",
                table: "Roles",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "InsertDate",
                value: new DateTime(2024, 6, 17, 19, 10, 8, 25, DateTimeKind.Utc).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "InsertDate",
                value: new DateTime(2024, 6, 17, 19, 10, 8, 25, DateTimeKind.Utc).AddTicks(7742));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                column: "InsertDate",
                value: new DateTime(2024, 6, 17, 19, 10, 8, 25, DateTimeKind.Utc).AddTicks(7743));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Style",
                table: "Roles",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "Relation",
                table: "Roles",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

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
                column: "InsertDate",
                value: new DateTime(2024, 6, 17, 13, 32, 58, 601, DateTimeKind.Utc).AddTicks(7000));
        }
    }
}
