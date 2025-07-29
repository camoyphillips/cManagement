namespace cManagement.Models
{
    /// <summary>
    /// Join table for many-to-many between Employee and Project.
    /// </summary>
    public class EmployeeProject
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
