// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class AddShelterRequestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShelterRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ShelterOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefugeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefugeeApplicantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShelterRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShelterRequests_RefugeeGroups_RefugeeGroupId",
                        column: x => x.RefugeeGroupId,
                        principalTable: "RefugeeGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShelterRequests_Refugees_RefugeeApplicantId",
                        column: x => x.RefugeeApplicantId,
                        principalTable: "Refugees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShelterRequests_ShelterOffers_ShelterOfferId",
                        column: x => x.ShelterOfferId,
                        principalTable: "ShelterOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShelterRequests_RefugeeApplicantId",
                table: "ShelterRequests",
                column: "RefugeeApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShelterRequests_RefugeeGroupId",
                table: "ShelterRequests",
                column: "RefugeeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ShelterRequests_ShelterOfferId",
                table: "ShelterRequests",
                column: "ShelterOfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShelterRequests");
        }
    }
}
