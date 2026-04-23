using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElRawabi_RealEstate_Backend.Migrations
{
    /// <inheritdoc />
    public partial class fixingConstructionStage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Stages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReportData",
                table: "Stages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "ReportData",
                table: "Stages");
        }
    }
}
