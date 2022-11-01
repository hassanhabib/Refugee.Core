using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class AddShelterRefugeeOccupantModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShelterRefugeeOccupants_Refugees_RefugeeId",
                table: "ShelterRefugeeOccupants");

            migrationBuilder.DropForeignKey(
                name: "FK_ShelterRefugeeOccupants_Shelters_ShelterId",
                table: "ShelterRefugeeOccupants");

            migrationBuilder.AddForeignKey(
                name: "FK_ShelterRefugeeOccupants_Refugees_RefugeeId",
                table: "ShelterRefugeeOccupants",
                column: "RefugeeId",
                principalTable: "Refugees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShelterRefugeeOccupants_Shelters_ShelterId",
                table: "ShelterRefugeeOccupants",
                column: "ShelterId",
                principalTable: "Shelters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShelterRefugeeOccupants_Refugees_RefugeeId",
                table: "ShelterRefugeeOccupants");

            migrationBuilder.DropForeignKey(
                name: "FK_ShelterRefugeeOccupants_Shelters_ShelterId",
                table: "ShelterRefugeeOccupants");

            migrationBuilder.AddForeignKey(
                name: "FK_ShelterRefugeeOccupants_Refugees_RefugeeId",
                table: "ShelterRefugeeOccupants",
                column: "RefugeeId",
                principalTable: "Refugees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShelterRefugeeOccupants_Shelters_ShelterId",
                table: "ShelterRefugeeOccupants",
                column: "ShelterId",
                principalTable: "Shelters",
                principalColumn: "Id");
        }
    }
}
