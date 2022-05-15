using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class AddRefugeeMigrationAddNationalities : Migration
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

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefugeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nationalities_Refugees_RefugeeId",
                        column: x => x.RefugeeId,
                        principalTable: "Refugees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_RefugeeId",
                table: "Nationalities",
                column: "RefugeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nationalities");

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
