// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class UpdateRefugeeModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Refugees_RefugeeId",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConditions_Refugees_RefugeeId",
                table: "MedicalConditions");

            migrationBuilder.DropForeignKey(
                name: "FK_Nationalities_Refugees_RefugeeId",
                table: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_Nationalities_RefugeeId",
                table: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_MedicalConditions_RefugeeId",
                table: "MedicalConditions");

            migrationBuilder.DropIndex(
                name: "IX_Languages_RefugeeId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Refugees");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Refugees");

            migrationBuilder.DropColumn(
                name: "SkillSets",
                table: "Refugees");

            migrationBuilder.DropColumn(
                name: "RefugeeId",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "RefugeeId",
                table: "MedicalConditions");

            migrationBuilder.DropColumn(
                name: "RefugeeId",
                table: "Languages");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Refugees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Refugees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Refugees");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Refugees");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Refugees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Refugees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillSets",
                table: "Refugees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefugeeId",
                table: "Nationalities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefugeeId",
                table: "MedicalConditions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefugeeId",
                table: "Languages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_RefugeeId",
                table: "Nationalities",
                column: "RefugeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConditions_RefugeeId",
                table: "MedicalConditions",
                column: "RefugeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_RefugeeId",
                table: "Languages",
                column: "RefugeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Refugees_RefugeeId",
                table: "Languages",
                column: "RefugeeId",
                principalTable: "Refugees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConditions_Refugees_RefugeeId",
                table: "MedicalConditions",
                column: "RefugeeId",
                principalTable: "Refugees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Nationalities_Refugees_RefugeeId",
                table: "Nationalities",
                column: "RefugeeId",
                principalTable: "Refugees",
                principalColumn: "Id");
        }
    }
}
