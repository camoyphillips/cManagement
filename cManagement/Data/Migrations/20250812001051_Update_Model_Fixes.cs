using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Model_Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_ApplicationUserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Trucks_TruckId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_AssignedDriverId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShipments_EmployeeId",
                table: "EmployeeShipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverShipments",
                table: "DriverShipments");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Shipments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TruckId1",
                table: "Shipments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "EmployeeShipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeShipmentId",
                table: "EmployeeShipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Drivers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedTruckId",
                table: "Drivers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverShipments",
                table: "DriverShipments",
                column: "DriverShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_AssignedDriverId",
                table: "Trucks",
                column: "AssignedDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ApplicationUserId",
                table: "Shipments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_TruckId1",
                table: "Shipments",
                column: "TruckId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApplicationUserId1",
                table: "Employees",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_DriverShipments_DriverId",
                table: "DriverShipments",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ApplicationUserId1",
                table: "Drivers",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_AssignedTruckId",
                table: "Drivers",
                column: "AssignedTruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_ApplicationUserId",
                table: "Drivers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_ApplicationUserId1",
                table: "Drivers",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Trucks_AssignedTruckId",
                table: "Drivers",
                column: "AssignedTruckId",
                principalTable: "Trucks",
                principalColumn: "TruckId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId1",
                table: "Employees",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_AspNetUsers_ApplicationUserId",
                table: "Shipments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Trucks_TruckId",
                table: "Shipments",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "TruckId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Trucks_TruckId1",
                table: "Shipments",
                column: "TruckId1",
                principalTable: "Trucks",
                principalColumn: "TruckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_ApplicationUserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_ApplicationUserId1",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Trucks_AssignedTruckId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId1",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_AspNetUsers_ApplicationUserId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Trucks_TruckId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Trucks_TruckId1",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_AssignedDriverId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ApplicationUserId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_TruckId1",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ApplicationUserId1",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverShipments",
                table: "DriverShipments");

            migrationBuilder.DropIndex(
                name: "IX_DriverShipments_DriverId",
                table: "DriverShipments");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_ApplicationUserId1",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_AssignedTruckId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "TruckId1",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "EmployeeShipmentId",
                table: "EmployeeShipments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "AssignedTruckId",
                table: "Drivers");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "EmployeeShipments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverShipments",
                table: "DriverShipments",
                columns: new[] { "DriverId", "ShipmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_AssignedDriverId",
                table: "Trucks",
                column: "AssignedDriverId",
                unique: true,
                filter: "[AssignedDriverId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShipments_EmployeeId",
                table: "EmployeeShipments",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_ApplicationUserId",
                table: "Drivers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Trucks_TruckId",
                table: "Shipments",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "TruckId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
