using cManagement.Models.Dtos;
using System.Collections.Generic; 

namespace cManagement.Models.ViewModels
{
    public class TruckDetailVM
    {
        public TruckDto Truck { get; set; } = null!;
        public DriverDto? AssignedDriver { get; set; }
        public List<ShipmentDto> ActiveShipments { get; set; } = new();
    }
}