using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cManagement.Data;

namespace cManagement.Models
{
    /// <summary>
    /// Represents an employee who belongs to one department and can work on multiple projects.
    /// </summary>
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>Foreign key to the Department.</summary>
        public int DepartmentId { get; set; }

        /// <summary>Navigation property to the employee's department.</summary>
        public Department? Department { get; set; }

        /// <summary>Many-to-many relationship with projects.</summary>
        public List<EmployeeProject> EmployeeProjects { get; set; } = new();

        /// <summary>One employee ↔ many shipments (through join).</summary>
        public ICollection<EmployeeShipment> EmployeeShipments { get; set; } = new List<EmployeeShipment>();

        /// <summary>Display-only property for full name.</summary>
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>Required link to AspNetUsers.Id (nvarchar).</summary>
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
