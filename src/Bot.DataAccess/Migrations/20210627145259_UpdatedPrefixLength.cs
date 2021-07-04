using Microsoft.EntityFrameworkCore.Migrations;

namespace Bot.DataAccess.Migrations
{
    public partial class UpdatedPrefixLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "Guilds",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "tinytext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "Guilds",
                type: "tinytext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
