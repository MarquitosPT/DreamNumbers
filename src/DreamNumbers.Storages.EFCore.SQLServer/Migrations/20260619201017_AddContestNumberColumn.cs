using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamNumbers.Storages.EFCore.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class AddContestNumberColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContestNumber",
                table: "EuroMillionDraws",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContestNumber",
                table: "EuroMillionDraws");
        }
    }
}
