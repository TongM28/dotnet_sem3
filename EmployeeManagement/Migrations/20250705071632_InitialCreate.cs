using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Address", "Department", "Email", "Gender", "HireDate", "Name", "PhoneNumber", "Position", "Role", "Salary", "Status" },
                values: new object[,]
                {
                    { "EM001", "123 Main St, City, Country", "HR", "john.carter@company.com", "Male", new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "John Carter", "1234567890", "HR Manager", "Admin", 3000m, "Active" },
                    { "EM002", "123 Main St, City, Country", "SC", "michael.bean@company.com", "Male", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Michael Bean", "2345678901", "Supply Chain Specialist", "Employee", 1300m, "Active" },
                    { "EM003", "123 Main St, City, Country", "MD", "jimmy.floy@company.com", "Male", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jimmy Floy", "3456789012", "Medical Doctor", "Employee", 2000m, "Active" },
                    { "EM004", "123 Main St, City, Country", "MD", "mary.brown@company.com", "Female", new DateTime(2020, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mary Brown", "4567890123", "Medical Doctor", "Employee", 2000m, "Active" },
                    { "EM005", "123 Main St, City, Country", "HR", "duc.tran@company.com", "Male", new DateTime(2018, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Duc Tran", "5678901234", "HR Director", "Admin", 12000m, "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
