using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSeatReservationToOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RESERVATION_SEAT_SeatId",
                table: "RESERVATION");

            migrationBuilder.DropIndex(
                name: "IX_RESERVATION_SeatId",
                table: "RESERVATION");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVATION_SeatId",
                table: "RESERVATION",
                column: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_RESERVATION_SEAT_SeatId",
                table: "RESERVATION",
                column: "SeatId",
                principalTable: "SEAT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RESERVATION_SEAT_SeatId",
                table: "RESERVATION");

            migrationBuilder.DropIndex(
                name: "IX_RESERVATION_SeatId",
                table: "RESERVATION");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVATION_SeatId",
                table: "RESERVATION",
                column: "SeatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RESERVATION_SEAT_SeatId",
                table: "RESERVATION",
                column: "SeatId",
                principalTable: "SEAT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
