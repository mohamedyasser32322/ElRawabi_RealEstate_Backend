using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElRawabi_RealEstate_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddHashPasswordToBuyer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashPassword",
                table: "Buyers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashPassword",
                table: "Buyers");
        }
    }
}
