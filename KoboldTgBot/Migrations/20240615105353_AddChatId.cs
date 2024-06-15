using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoboldTgBot.Migrations
{
    public partial class AddChatId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "Messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Messages");
        }
    }
}
