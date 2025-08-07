using System.ComponentModel.DataAnnotations.Schema;

namespace cManagement.Models
{
    /// <summary>
    /// Join table for many-to-many between Employee and Project.
    /// </summary>
    public class EmployeeProject
    {
        // Composite Key (defined in DbContext)
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; } = null!;
    }
}
