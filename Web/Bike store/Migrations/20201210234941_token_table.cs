using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace NoNameBikes.Migrations
{
    public partial class token_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordRecoveryToken",
                columns: table => new
                {
                    TokenID = table.Column<int>(nullable: false, comment: "Primary key for Token.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: false, comment: "The Token value."),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())", comment: "The date and time at which the token created."),
                    ExpireDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "The date and time at which the token is expired."),
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecoveryToken", x => x.TokenID);
                },
                comment: "Keeps track of token for password recovery");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
