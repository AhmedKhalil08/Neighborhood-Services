using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neighborhood.Services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Checking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Bookings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Bookings");
        }
    }
}
