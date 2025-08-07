using cManagement.DTOs;
using cManagement.Models.Dtos;
using cManagement.Services;
using System.Collections.Generic;

namespace cManagement.Models.ViewModels
{
    /// <summary>
    /// Combines project data with assigned employees and related shipments.
    /// </summary>
    public class ProjectDetailVM
    {
        public ProjectDto Project { get; set; } = null!;
        public List<EmployeeDto> AssignedEmployees { get; set; } = new();
        public List<ShipmentDto> RelatedShipments { get; set; } = new();
    }
}

