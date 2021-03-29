using Microsoft.EntityFrameworkCore.Migrations;

namespace ErogeHelper.Server.Migrations
{
    public partial class SubtitleHashString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashString",
                table: "Subtitles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashString",
                table: "Subtitles");
        }
    }
}
