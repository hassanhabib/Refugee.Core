// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class AddAllowedPetMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllowedPet_Shelters_ShelterId",
                table: "AllowedPet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllowedPet",
                table: "AllowedPet");

            migrationBuilder.RenameTable(
                name: "AllowedPet",
                newName: "AllowedPets");

            migrationBuilder.RenameIndex(
                name: "IX_AllowedPet_ShelterId",
                table: "AllowedPets",
                newName: "IX_AllowedPets_ShelterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllowedPets",
                table: "AllowedPets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AllowedPets_Shelters_ShelterId",
                table: "AllowedPets",
                column: "ShelterId",
                principalTable: "Shelters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllowedPets_Shelters_ShelterId",
                table: "AllowedPets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllowedPets",
                table: "AllowedPets");

            migrationBuilder.RenameTable(
                name: "AllowedPets",
                newName: "AllowedPet");

            migrationBuilder.RenameIndex(
                name: "IX_AllowedPets_ShelterId",
                table: "AllowedPet",
                newName: "IX_AllowedPet_ShelterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllowedPet",
                table: "AllowedPet",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AllowedPet_Shelters_ShelterId",
                table: "AllowedPet",
                column: "ShelterId",
                principalTable: "Shelters",
                principalColumn: "Id");
        }
    }
}
