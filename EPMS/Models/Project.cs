using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;

namespace EPMS.Models
{
    // This class is for saving project data in the database
    // One project can have many employees working on it
    public class Project
    {
        // This is the ID for the project (like a unique number)
        [Key]
        public int ProjectId { get; set; }

        // This is the name of the project
        // It is required and cannot be more than 100 characters
        [Required(ErrorMessage = "Project name is required.")]
        [MaxLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
        public string ProjectName { get; set; }

        // This is the description of the project (optional)
        // It can be empty but if written, it must be under 500 characters
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        // This is the list of employees working on this project
        // It is used to make many-to-many connection between employee and project
        public List<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
