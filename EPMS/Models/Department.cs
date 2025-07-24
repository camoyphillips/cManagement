using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPMS.Models
{
    // Represents a department in the system
    // One department can have multiple employees (1-to-many relationship)
    public class Department
    {
        // Primary key for the department
        [Key]
        public int DepartmentId { get; set; }

        // Name of the department (required and max 100 characters)
        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string DepartmentName { get; set; }

        // List of employees that belong to this department
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
