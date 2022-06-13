// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class AddRefugeePetModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefugeePets",
                columns: table => new
                {
                    RefugeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefugeePets", x => new { x.RefugeeId, x.PetId });
                    table.ForeignKey(
                        name: "FK_RefugeePets_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefugeePets_Refugees_RefugeeId",
                        column: x => x.RefugeeId,
                        principalTable: "Refugees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefugeePets_PetId",
                table: "RefugeePets",
                column: "PetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefugeePets");
        }
    }
}
