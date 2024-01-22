using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class v5_enums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_IdentifiedString_StatusId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_IdentifiedString_StatusId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "IdentifiedString");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StatusId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PermissionLevel",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PermissionLevel",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IdentifiedString",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeUsername = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentifiedString", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentifiedString_Employees_EmployeeUsername",
                        column: x => x.EmployeeUsername,
                        principalTable: "Employees",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StatusId",
                table: "Projects",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedString_EmployeeUsername",
                table: "IdentifiedString",
                column: "EmployeeUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_IdentifiedString_StatusId",
                table: "Projects",
                column: "StatusId",
                principalTable: "IdentifiedString",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_IdentifiedString_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "IdentifiedString",
                principalColumn: "Id");
        }
    }
}
