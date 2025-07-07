using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        [Key]
        [Required(ErrorMessage = "Employee ID is required")]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
        public decimal Salary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required]
        public string Status { get; set; } // Active or Inactive

        [Required]
        public string Role { get; set; } // Admin or Employee

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}