// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class AddRefugeeGroupMembership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refugees_RefugeeGroups_RefugeeGroupId",
                table: "Refugees");

            migrationBuilder.DropIndex(
                name: "IX_Refugees_RefugeeGroupId",
                table: "Refugees");

            migrationBuilder.DropColumn(
                name: "RefugeeGroupId",
                table: "Refugees");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RefugeeGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RefugeeGroupMainRepresentativeId",
                table: "RefugeeGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "RefugeeGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RefugeeGroupMemberships",
                columns: table => new
                {
                    RefugeeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefugeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDecisionMaker = table.Column<bool>(type: "bit", nullable: false),
                    IsRefugeeGroupRepresentative = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefugeeGroupMemberships", x => new { x.RefugeeId, x.RefugeeGroupId });
                    table.ForeignKey(
                        name: "FK_RefugeeGroupMemberships_RefugeeGroups_RefugeeGroupId",
                        column: x => x.RefugeeGroupId,
                        principalTable: "RefugeeGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefugeeGroupMemberships_Refugees_RefugeeId",
                        column: x => x.RefugeeId,
                        principalTable: "Refugees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefugeeGroups_RefugeeGroupMainRepresentativeId",
                table: "RefugeeGroups",
                column: "RefugeeGroupMainRepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_RefugeeGroupMemberships_RefugeeGroupId",
                table: "RefugeeGroupMemberships",
                column: "RefugeeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefugeeGroups_Refugees_RefugeeGroupMainRepresentativeId",
                table: "RefugeeGroups",
                column: "RefugeeGroupMainRepresentativeId",
                principalTable: "Refugees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefugeeGroups_Refugees_RefugeeGroupMainRepresentativeId",
                table: "RefugeeGroups");

            migrationBuilder.DropTable(
                name: "RefugeeGroupMemberships");

            migrationBuilder.DropIndex(
                name: "IX_RefugeeGroups_RefugeeGroupMainRepresentativeId",
                table: "RefugeeGroups");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RefugeeGroups");

            migrationBuilder.DropColumn(
                name: "RefugeeGroupMainRepresentativeId",
                table: "RefugeeGroups");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "RefugeeGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "RefugeeGroupId",
                table: "Refugees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refugees_RefugeeGroupId",
                table: "Refugees",
                column: "RefugeeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Refugees_RefugeeGroups_RefugeeGroupId",
                table: "Refugees",
                column: "RefugeeGroupId",
                principalTable: "RefugeeGroups",
                principalColumn: "Id");
        }
    }
}
