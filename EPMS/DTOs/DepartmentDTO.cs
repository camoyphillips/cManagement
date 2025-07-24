namespace EPMS.DTOs
{
    // This is a class for sending department data
    public class DepartmentDTO
    {
        // This is the ID of the department (like its unique number)
        public int DepartmentId { get; set; }

        // This is the name of the department
        // It is used to show the department in dropdowns and lists
        public string DepartmentName { get; set; } = null!;
    }
}
