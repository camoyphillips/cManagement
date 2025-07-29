using System.ComponentModel.DataAnnotations.Schema;

namespace EPMS.Models
{
    // This class connects Employees and Projects
    // It is used for many-to-many relationship (like a bridge table)
    public class EmployeeProject
    {
        // ID of the employee (part of the key)
        public int EmployeeId { get; set; }

        // Link to the Employee object (to get full employee info)
        public Employee Employee { get; set; }

        // ID of the project (other part of the key)
        public int ProjectId { get; set; }

        // Link to the Project object (to get full project info)
        public Project Project { get; set; }
    }
}
