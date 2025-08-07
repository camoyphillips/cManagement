using cManagement.Models.Dtos;

namespace cManagement.Models.ViewModels
{
    /// <summary>
    /// Combines shipment data with related truck and assigned drivers for view rendering.
    /// </summary>
    public class ShipmentDetailVM
    {
        public ShipmentDto Shipment { get; set; } = null!;
        public string? AssignedTruckModel { get; set; }
        public List<DriverShipmentDto> AssignedDrivers { get; set; } = new List<DriverShipmentDto>();

       
    }
}
