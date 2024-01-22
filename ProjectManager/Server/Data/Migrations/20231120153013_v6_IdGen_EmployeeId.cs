using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class v6_IdGen_EmployeeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCenter_Employees_ManagerUsername",
                table: "CostCenter");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTaskItem_Employees_AssigneesUsername",
                table: "EmployeeTaskItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_ProjectManagerUsername",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectManagerUsername",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTaskItem",
                table: "EmployeeTaskItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_CostCenter_ManagerUsername",
                table: "CostCenter");

            migrationBuilder.DropColumn(
                name: "ProjectManagerUsername",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AssigneesUsername",
                table: "EmployeeTaskItem");

            migrationBuilder.DropColumn(
                name: "ManagerUsername",
                table: "CostCenter");

            migrationBuilder.AddColumn<int>(
                name: "ProjectManagerId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssigneesId",
                table: "EmployeeTaskItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "CostCenter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTaskItem",
                table: "EmployeeTaskItem",
                columns: new[] { "AssigneesId", "TasksId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Username",
                table: "Employees",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCenter_ManagerId",
                table: "CostCenter",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenter_Employees_ManagerId",
                table: "CostCenter",
                column: "ManagerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTaskItem_Employees_AssigneesId",
                table: "EmployeeTaskItem",
                column: "AssigneesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Employees_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCenter_Employees_ManagerId",
                table: "CostCenter");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTaskItem_Employees_AssigneesId",
                table: "EmployeeTaskItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTaskItem",
                table: "EmployeeTaskItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Username",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_CostCenter_ManagerId",
                table: "CostCenter");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AssigneesId",
                table: "EmployeeTaskItem");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "CostCenter");

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagerUsername",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssigneesUsername",
                table: "EmployeeTaskItem",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagerUsername",
                table: "CostCenter",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTaskItem",
                table: "EmployeeTaskItem",
                columns: new[] { "AssigneesUsername", "TasksId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerUsername",
                table: "Projects",
                column: "ProjectManagerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenter_ManagerUsername",
                table: "CostCenter",
                column: "ManagerUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCenter_Employees_ManagerUsername",
                table: "CostCenter",
                column: "ManagerUsername",
                principalTable: "Employees",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTaskItem_Employees_AssigneesUsername",
                table: "EmployeeTaskItem",
                column: "AssigneesUsername",
                principalTable: "Employees",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Employees_ProjectManagerUsername",
                table: "Projects",
                column: "ProjectManagerUsername",
                principalTable: "Employees",
                principalColumn: "Username");
        }
    }
}
