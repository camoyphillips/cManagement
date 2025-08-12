using System.ComponentModel.DataAnnotations;

namespace cManagement.Models.ViewModels
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        [Required, MaxLength(200)]
        public string DepartmentName { get; set; } = string.Empty;
    }
}
