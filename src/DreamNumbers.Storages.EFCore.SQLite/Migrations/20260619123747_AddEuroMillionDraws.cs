using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamNumbers.Storages.EFCore.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class AddEuroMillionDraws : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EuroMillionDraws",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DrawNumber = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Numbers = table.Column<string>(type: "TEXT", nullable: false),
                    Stars = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EuroMillionDraws", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EuroMillionDraws");
        }
    }
}
