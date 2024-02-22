using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoNameBikes.Migrations
{
    public partial class newTablesDateTypeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogIn");

            migrationBuilder.DropTable(
                name: "PasswordRecoveryToken");

            migrationBuilder.CreateTable(
                name: "LogIn",
                columns: table => new
                {
                    LoginId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogIn", x => x.LoginId);
                });

            migrationBuilder.CreateTable(
                name: "PasswordRecoveryToken",
                columns: table => new
                {
                    TokenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExpireDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecoveryToken", x => x.TokenId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogIn");

            migrationBuilder.DropTable(
                name: "PasswordRecoveryToken");
        }
    }
}
