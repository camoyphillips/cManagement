using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cManagement.Models
{
    /// <summary>
    /// Represents a department which can have multiple employees (1-M relationship).
    /// </summary>
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string DepartmentName { get; set; } = string.Empty;

        public List<Employee> Employees { get; set; } = new();
    }
}
