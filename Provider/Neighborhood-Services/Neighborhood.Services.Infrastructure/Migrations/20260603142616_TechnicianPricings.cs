using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neighborhood.Services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TechnicianPricings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TechnicianPricings_TechnicianId",
                table: "TechnicianPricings");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianPricings_TechnicianId_ProblemTypeId",
                table: "TechnicianPricings",
                columns: new[] { "TechnicianId", "ProblemTypeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TechnicianPricings_TechnicianId_ProblemTypeId",
                table: "TechnicianPricings");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianPricings_TechnicianId",
                table: "TechnicianPricings",
                column: "TechnicianId");
        }
    }
}
