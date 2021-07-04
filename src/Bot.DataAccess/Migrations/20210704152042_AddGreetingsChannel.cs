using Microsoft.EntityFrameworkCore.Migrations;

namespace Bot.DataAccess.Migrations
{
    public partial class AddGreetingsChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "GreetingsChannelId",
                table: "Guilds",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GreetingsChannelId",
                table: "Guilds");
        }
    }
}
