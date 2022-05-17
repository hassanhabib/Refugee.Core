// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class AddRefugeeGroupsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Proficiency",
                table: "Languages");

            migrationBuilder.AddColumn<Guid>(
                name: "RefugeeGroupId",
                table: "Refugees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FluencyLevel",
                table: "Languages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RefugeeGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefugeeGroups", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refugees_RefugeeGroups_RefugeeGroupId",
                table: "Refugees");

            migrationBuilder.DropTable(
                name: "RefugeeGroups");

            migrationBuilder.DropIndex(
                name: "IX_Refugees_RefugeeGroupId",
                table: "Refugees");

            migrationBuilder.DropColumn(
                name: "RefugeeGroupId",
                table: "Refugees");

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
