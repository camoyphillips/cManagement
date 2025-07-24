using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPMS.Models
{
    // This class is for Employee table in the database
    // One employee belongs to one department and can work on many projects
    public class Employee
    {
        // Unique ID for the employee (Primary Key)
        [Key]
        public int EmployeeId { get; set; }

        // First name of employee (required, max 50 characters)
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        // Last name of employee (required, max 50 characters)
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        // Email of the employee (required, must be valid, max 100 characters)
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        // ID of the department this employee belongs to (foreign key)
        public int DepartmentId { get; set; }

        // Link to the Department details
        public Department Department { get; set; }

        // List of all projects this employee is working on (many-to-many)
        public List<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

        // Full name of employee (not stored in database, just for display)
        public string FullName => FirstName + " " + LastName;
    }
}
