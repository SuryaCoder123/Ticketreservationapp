using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ticketreservationapp.Migrations
{
    /// <inheritdoc />
    public partial class surya1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "AvailableSeats", "Date", "Name", "TotalSeats", "Venue" },
                values: new object[,]
                {
                    { 1, 200, new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tech Conference 2024", 200, "New York Hall" },
                    { 2, 150, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comedy Show with John Doe", 150, "LA Theater" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
