﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasinoDealer2.Migrations
{
    /// <inheritdoc />
    public partial class BJTournamentRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentStreak",
                table: "BlackJackTournamentRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentStreak",
                table: "BlackJackTournamentRecords");
        }
    }
}
