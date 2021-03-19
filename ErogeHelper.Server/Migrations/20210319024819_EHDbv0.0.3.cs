using Microsoft.EntityFrameworkCore.Migrations;

namespace ErogeHelper.Server.Migrations
{
    public partial class EHDbv003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SubtitleLevel",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TermLevel",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubtitleLevel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TermLevel",
                table: "Users");
        }
    }
}
