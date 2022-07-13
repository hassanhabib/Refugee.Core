using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugeeLand.Core.Api.Migrations
{
    public partial class UpdateRefugeeGroupMembershipFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RefugeeGroupMemberships",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "RefugeeGroupMemberships",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "RefugeeGroupMemberships",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "RefugeeGroupMemberships",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RefugeeGroupMemberships");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RefugeeGroupMemberships");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "RefugeeGroupMemberships");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RefugeeGroupMemberships");
        }
    }
}
