using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasinoDealer2.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuestionAddTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameType",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameType",
                table: "Questions");
        }
    }
}
