using Microsoft.EntityFrameworkCore.Migrations;

namespace ErogeHelper.Server.Migrations
{
    public partial class SubtitleUnixTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreationTimeUnix",
                table: "Subtitles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RevisionTimeUnix",
                table: "Subtitles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTimeUnix",
                table: "Subtitles");

            migrationBuilder.DropColumn(
                name: "RevisionTimeUnix",
                table: "Subtitles");
        }
    }
}
