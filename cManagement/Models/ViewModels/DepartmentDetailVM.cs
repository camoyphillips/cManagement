using cManagement.DTOs;
using cManagement.Models.Dtos;
using System.Collections.Generic;

namespace cManagement.Models.ViewModels
{
    /// <summary>
    /// Combines department data with associated employees for view rendering.
    /// </summary>
    public class DepartmentDetailVM
    {
        public DepartmentDto Department { get; set; } = null!;
        public List<EmployeeDto> Employees { get; set; } = new();
        public int TotalEmployees => Employees.Count;

        // Add metrics here
        public int TotalAssignedShipments { get; set; }
    }
}
