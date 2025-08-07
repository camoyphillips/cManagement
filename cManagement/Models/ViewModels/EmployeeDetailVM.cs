using cManagement.DTOs;
using cManagement.Models.Dtos;
using System.Collections.Generic;

namespace cManagement.Models.ViewModels
{
    /// <summary>
    /// Combines employee data with assigned shipments and department info.
    /// </summary>
    public class EmployeeDetailVM
    {
        public EmployeeDto Employee { get; set; } = null!;
        public string? DepartmentName { get; set; }
        public List<ShipmentDto> AssignedShipments { get; set; } = new();
    }
}
