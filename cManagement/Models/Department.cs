using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cManagement.Models
{
    /// <summary>
    /// Represents a department which can have multiple employees (1-M relationship).
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Primary key for the department.
        /// </summary>
        [Key]
        public int DepartmentId { get; set; }

        /// <summary>
        /// Name of the department (required, max 100 characters).
        /// </summary>
        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Collection of employees assigned to this department.
        /// </summary>
        public List<Employee> Employees { get; set; } = new();
    }
}
