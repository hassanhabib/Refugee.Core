// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class UpdateAuditableProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShelterRefugeeOccupants",
                table: "ShelterRefugeeOccupants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefugeePets",
                table: "RefugeePets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefugeeGroupMemberships",
                table: "RefugeeGroupMemberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetMedicalConditions",
                table: "PetMedicalConditions");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "ShelterRequests",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ShelterRequests",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "ShelterOffers",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ShelterOffers",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Refugees",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Refugees",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "RefugeeGroups",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "RefugeeGroups",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "RefugeeGroupMemberships",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "RefugeeGroupMemberships",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Hosts",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Hosts",
                newName: "CreatedByUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Shelters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Shelters",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "Shelters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Shelters",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ShelterRefugeeOccupants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "RefugeePets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "RefugeePets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "RefugeePets",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "RefugeePets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "RefugeePets",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "RefugeeGroupMemberships",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PetMedicalConditions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "PetMedicalConditions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "PetMedicalConditions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "PetMedicalConditions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "PetMedicalConditions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Nationalities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Nationalities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "Nationalities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Nationalities",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "MedicalConditions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "MedicalConditions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "MedicalConditions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "MedicalConditions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Languages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Languages",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "Languages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Languages",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "AllowedPets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AllowedPets",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "AllowedPets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AllowedPets",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShelterRefugeeOccupants",
                table: "ShelterRefugeeOccupants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefugeePets",
                table: "RefugeePets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefugeeGroupMemberships",
                table: "RefugeeGroupMemberships",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetMedicalConditions",
                table: "PetMedicalConditions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShelterRefugeeOccupants_ShelterId",
                table: "ShelterRefugeeOccupants",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_RefugeePets_RefugeeId",
                table: "RefugeePets",
                column: "RefugeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RefugeeGroupMemberships_RefugeeId",
                table: "RefugeeGroupMemberships",
                column: "RefugeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PetMedicalConditions_PetId",
                table: "PetMedicalConditions",
                column: "PetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShelterRefugeeOccupants",
                table: "ShelterRefugeeOccupants");

            migrationBuilder.DropIndex(
                name: "IX_ShelterRefugeeOccupants_ShelterId",
                table: "ShelterRefugeeOccupants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefugeePets",
                table: "RefugeePets");

            migrationBuilder.DropIndex(
                name: "IX_RefugeePets_RefugeeId",
                table: "RefugeePets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefugeeGroupMemberships",
                table: "RefugeeGroupMemberships");

            migrationBuilder.DropIndex(
                name: "IX_RefugeeGroupMemberships_RefugeeId",
                table: "RefugeeGroupMemberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetMedicalConditions",
                table: "PetMedicalConditions");

            migrationBuilder.DropIndex(
                name: "IX_PetMedicalConditions_PetId",
                table: "PetMedicalConditions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Shelters");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Shelters");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Shelters");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Shelters");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShelterRefugeeOccupants");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RefugeePets");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RefugeePets");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RefugeePets");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "RefugeePets");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RefugeePets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RefugeeGroupMemberships");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PetMedicalConditions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "PetMedicalConditions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PetMedicalConditions");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "PetMedicalConditions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PetMedicalConditions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "MedicalConditions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MedicalConditions");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "MedicalConditions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MedicalConditions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "AllowedPets");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AllowedPets");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "AllowedPets");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AllowedPets");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "ShelterRequests",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "ShelterRequests",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "ShelterOffers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "ShelterOffers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "Refugees",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Refugees",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "RefugeeGroups",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "RefugeeGroups",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "RefugeeGroupMemberships",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "RefugeeGroupMemberships",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "Hosts",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Hosts",
                newName: "CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShelterRefugeeOccupants",
                table: "ShelterRefugeeOccupants",
                columns: new[] { "ShelterId", "RefugeeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefugeePets",
                table: "RefugeePets",
                columns: new[] { "RefugeeId", "PetId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefugeeGroupMemberships",
                table: "RefugeeGroupMemberships",
                columns: new[] { "RefugeeId", "RefugeeGroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetMedicalConditions",
                table: "PetMedicalConditions",
                columns: new[] { "PetId", "MedicalConditionId" });
        }
    }
}
