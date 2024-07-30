using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasinoDealer2.Migrations
{
    /// <inheritdoc />
    public partial class UPdatedRouletteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionsAR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<double>(type: "float", nullable: false),
                    CorrectAnswer = table.Column<double>(type: "float", nullable: false),
                    GameType = table.Column<int>(type: "int", nullable: false),
                    StraitUp = table.Column<int>(type: "int", nullable: false),
                    Split = table.Column<int>(type: "int", nullable: false),
                    Corner = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<int>(type: "int", nullable: false),
                    SixLine = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    IncorrectStreak = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsAR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsAR_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsAR_UserId",
                table: "QuestionsAR",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionsAR");
        }
    }
}
