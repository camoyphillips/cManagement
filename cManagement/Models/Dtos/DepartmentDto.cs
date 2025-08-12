namespace cManagement.DTOs
{
    // This is a class for sending department data
    public class DepartmentDTO
    {
        // This is the ID of the department 
        public int DepartmentId { get; set; }

        // It is used to show the department in dropdowns and lists
        public string DepartmentName { get; set; } = null!;
    }
}
