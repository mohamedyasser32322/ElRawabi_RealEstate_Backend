using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElRawabi_RealEstate_Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixUniqueIndexesWithSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Units_UnitNumber_FloorId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleName",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Name",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Buyers_Email",
                table: "Buyers");

            migrationBuilder.DropIndex(
                name: "IX_Buyers_NationalId",
                table: "Buyers");

            migrationBuilder.DropIndex(
                name: "IX_Buyers_PhoneNumber",
                table: "Buyers");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Units_UnitNumber_FloorId",
                table: "Units",
                columns: new[] { "UnitNumber", "FloorId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_Email",
                table: "Buyers",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_NationalId",
                table: "Buyers",
                column: "NationalId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_PhoneNumber",
                table: "Buyers",
                column: "PhoneNumber",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Units_UnitNumber_FloorId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleName",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Name",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Buyers_Email",
                table: "Buyers");

            migrationBuilder.DropIndex(
                name: "IX_Buyers_NationalId",
                table: "Buyers");

            migrationBuilder.DropIndex(
                name: "IX_Buyers_PhoneNumber",
                table: "Buyers");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_UnitNumber_FloorId",
                table: "Units",
                columns: new[] { "UnitNumber", "FloorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_Email",
                table: "Buyers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_NationalId",
                table: "Buyers",
                column: "NationalId",
                unique: true,
                filter: "[NationalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_PhoneNumber",
                table: "Buyers",
                column: "PhoneNumber",
                unique: true);
        }
    }
}
