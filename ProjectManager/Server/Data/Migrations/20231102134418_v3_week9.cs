using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class v3_week9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Tasks_TaskId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_TaskId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "CostCenter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmployeeTaskItem",
                columns: table => new
                {
                    AssigneesUsername = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TasksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTaskItem", x => new { x.AssigneesUsername, x.TasksId });
                    table.ForeignKey(
                        name: "FK_EmployeeTaskItem_Employees_AssigneesUsername",
                        column: x => x.AssigneesUsername,
                        principalTable: "Employees",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTaskItem_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCenter_ProjectId",
                table: "CostCenter",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTaskItem_TasksId",
                table: "EmployeeTaskItem",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenter_Projects_ProjectId",
                table: "CostCenter",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCenter_Projects_ProjectId",
                table: "CostCenter");

            migrationBuilder.DropTable(
                name: "EmployeeTaskItem");

            migrationBuilder.DropIndex(
                name: "IX_CostCenter_ProjectId",
                table: "CostCenter");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CostCenter");

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TaskId",
                table: "Employees",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Tasks_TaskId",
                table: "Employees",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
