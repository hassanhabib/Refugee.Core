using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class AddRefugeeMigrationAddMiddleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Refugees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Refugees");
        }
    }
}
