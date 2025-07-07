using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = "EM001",
                    Name = "John Carter",
                    Gender = "Male",
                    Position = "HR Manager",
                    Department = "HR",
                    Salary = 3000,
                    HireDate = new DateTime(2020, 1, 15),
                    Status = "Active",
                    Role = "Admin",
                    Email = "john.carter@company.com",
                    PhoneNumber = "1234567890",
                    Address = "123 Main St, City, Country"
                },
                new Employee
                {
                    EmployeeId = "EM002",
                    Name = "Michael Bean",
                    Gender = "Male",
                    Position = "Supply Chain Specialist",
                    Department = "SC",
                    Salary = 1300,
                    HireDate = new DateTime(2021, 3, 10),
                    Status = "Active",
                    Role = "Employee",
                    Email = "michael.bean@company.com",
                    PhoneNumber = "2345678901",
                    Address = "123 Main St, City, Country"
                },
                new Employee
                {
                    EmployeeId = "EM003",
                    Name = "Jimmy Floy",
                    Gender = "Male",
                    Position = "Medical Doctor",
                    Department = "MD",
                    Salary = 2000,
                    HireDate = new DateTime(2019, 5, 20),
                    Status = "Active",
                    Role = "Employee",
                    Email = "jimmy.floy@company.com",
                    PhoneNumber = "3456789012",
                    Address = "123 Main St, City, Country"
                },
                new Employee
                {
                    EmployeeId = "EM004",
                    Name = "Mary Brown",
                    Gender = "Female",
                    Position = "Medical Doctor",
                    Department = "MD",
                    Salary = 2000,
                    HireDate = new DateTime(2020, 7, 12),
                    Status = "Active",
                    Role = "Employee",
                    Email = "mary.brown@company.com",
                    PhoneNumber = "4567890123",
                    Address = "123 Main St, City, Country"
                },
                new Employee
                {
                    EmployeeId = "EM005",
                    Name = "Duc Tran",
                    Gender = "Male",
                    Position = "HR Director",
                    Department = "HR",
                    Salary = 12000,
                    HireDate = new DateTime(2018, 9, 5),
                    Status = "Active",
                    Role = "Admin",
                    Email = "duc.tran@company.com",
                    PhoneNumber = "5678901234",
                    Address = "123 Main St, City, Country"
                }
            );
        }
    }
}