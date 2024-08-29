using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_Challenge.Migrations
{
    /// <inheritdoc />
    public partial class PrimerMigra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PermissionTypes",
                newName: "PermissionTypeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Permissions",
                newName: "PermissionId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "PermissionTypeId",
                table: "PermissionTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "Permissions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Employees",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Employees",
                newName: "Id");
        }
    }
}
