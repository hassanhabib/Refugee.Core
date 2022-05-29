using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class UpdateLanguagesFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Proficiency",
                table: "Languages");

            migrationBuilder.AddColumn<int>(
                name: "FluencyLevel",
                table: "Languages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FluencyLevel",
                table: "Languages");

            migrationBuilder.AddColumn<string>(
                name: "Proficiency",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
