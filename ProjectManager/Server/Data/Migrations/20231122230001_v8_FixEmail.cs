using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class v8_FixEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Employees",
                newName: "EmailAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_Username",
                table: "Employees",
                newName: "IX_Employees_EmailAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Employees",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_EmailAddress",
                table: "Employees",
                newName: "IX_Employees_Username");
        }
    }
}
