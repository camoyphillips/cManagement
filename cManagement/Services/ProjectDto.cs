using System.ComponentModel.DataAnnotations;

namespace cManagement.Services
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }

        [Required, MaxLength(200)]
        public string ProjectName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
