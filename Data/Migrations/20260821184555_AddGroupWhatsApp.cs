using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HikeJordanDotNet.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupWhatsApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "Users",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "Users");
        }
    }
}
