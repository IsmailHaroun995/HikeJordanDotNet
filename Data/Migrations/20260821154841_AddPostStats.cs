using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HikeJordanDotNet.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DistanceKm",
                table: "Posts",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ElevationGainM",
                table: "Posts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanceKm",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ElevationGainM",
                table: "Posts");
        }
    }
}
