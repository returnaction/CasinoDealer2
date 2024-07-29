using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasinoDealer2.Migrations
{
    /// <inheritdoc />
    public partial class addTournamentForBJ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlackJackTournamentRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LongestStreak = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackJackTournamentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackJackTournamentRecords_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlackJackTournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<double>(type: "float", nullable: false),
                    UserAnswer = table.Column<double>(type: "float", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    TimeTaken = table.Column<TimeSpan>(type: "time", nullable: false),
                    CorrectStreak = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackJackTournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackJackTournaments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlackJackTournamentRecords_UserId",
                table: "BlackJackTournamentRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackJackTournaments_UserId",
                table: "BlackJackTournaments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackJackTournamentRecords");

            migrationBuilder.DropTable(
                name: "BlackJackTournaments");
        }
    }
}
