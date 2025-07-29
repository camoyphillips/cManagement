namespace EPMS.DTOs
{
    // This class is for project data

    public class ProjectDTO
    {
        // This is the ID of the project
        // It is a unique number for each project
        public int ProjectId { get; set; }

        // This is the name of the project
        public string ProjectName { get; set; } = null!;

        // This is the description of the project
        public string? Description { get; set; }
    }
}
