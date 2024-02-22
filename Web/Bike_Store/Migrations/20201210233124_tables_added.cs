using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoNameBikes.Migrations
{
    public partial class tables_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogIn",
                columns: table => new
                {
                    LoginID = table.Column<int>(nullable: false, comment: "Primary key for login records.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())", comment: "The date and time at which the login occurred."),
                    CustomerId = table.Column<int>(nullable: false, comment: "The user who logged in."),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogIn", x => x.LoginID);
                },
                comment: "Keeps track of who logged in");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}
