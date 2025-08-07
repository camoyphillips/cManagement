using cManagement.Models.Dtos;

namespace cManagement.Models.ViewModels
{
    /// <summary>
    /// Combines driver data with related truck and assigned shipments for view rendering.
    /// </summary>
    public class DriverDetailVM
    {
        public DriverDto Driver { get; set; } = null!;
        public string? AssignedTruckModel { get; set; }
        public List<ShipmentDto> AssignedShipments { get; set; } = new();
    }
}
