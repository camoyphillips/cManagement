using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cManagement.Models
{
    /// <summary>
    /// Represents a project that can include multiple employees.
    /// </summary>
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required, MaxLength(100)]
        public string ProjectName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Many-to-many relationship to employees.
        /// </summary>
        public List<EmployeeProject> EmployeeProjects { get; set; } = new();
    }
}
