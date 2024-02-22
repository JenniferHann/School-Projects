using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoNameBikes.Migrations
{
    public partial class TokenExpiryTypeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "PasswordRecoveryToken");

            migrationBuilder.AddColumn<bool>(
                name: "Expired",
                table: "PasswordRecoveryToken",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expired",
                table: "PasswordRecoveryToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "PasswordRecoveryToken",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
