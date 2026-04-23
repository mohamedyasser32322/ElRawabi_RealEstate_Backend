using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElRawabi_RealEstate_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOldNewValuesToActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewValues",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValues",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewValues",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "OldValues",
                table: "ActivityLogs");
        }
    }
}
