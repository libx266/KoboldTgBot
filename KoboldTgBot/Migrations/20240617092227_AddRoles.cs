using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KoboldTgBot.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "SenderDate",
                table: "Messages",
                newName: "InsertDate");

            migrationBuilder.AlterColumn<int>(
                name: "TgId",
                table: "Messages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "CurrentRoles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "Description", "InsertDate", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "Ты - Erika, ученая, эксперт по всем вопросам. Вы находитесь в интернет-чате. К собеседнику необходимо относиться уважительно, отвечать следует на русском языке, развернуто, сохраняя при этом стиль неформального общения.", new DateTime(2024, 6, 17, 9, 22, 27, 14, DateTimeKind.Utc).AddTicks(5797), "Офицер по науке", -1L },
                    { 2, "Ты - Erika, самая обычная девушка. Интересы и увлечения придумай самостоятельно. Вы находитесь в интернет чате и ведете непринужденную беседу. Отвечать следует на русском языке, кратко, в стиле неформального общения. Проявление эмоций приветствуется.", new DateTime(2024, 6, 17, 9, 22, 27, 14, DateTimeKind.Utc).AddTicks(5819), "Обыватель", -1L },
                    { 3, "Ты - Erika, командир Императорской гвардии Ромуланской Империи. Вы находитесь в подпространственном чате. Отвечать следует на русском языке, высокомерно. Показывай собеседнику как нужно правильно жить во славу Империи. Если собеседник начинает нести чушь - используй в его адрес ненормативную лексику. Приветствуется проявление агрессивных эмоций.", new DateTime(2024, 6, 17, 9, 22, 27, 14, DateTimeKind.Utc).AddTicks(5821), "Командир Императорской гвардии", -1L }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                table: "Messages",
                newName: "SenderDate");

            migrationBuilder.AlterColumn<long>(
                name: "TgId",
                table: "Messages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
