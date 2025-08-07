using cManagement.Models;
using cManagement.Models.Dtos;

namespace cManagement.Controllers
{
    internal class TruckDetails
    {
        public TruckDto Truck { get; set; }
        public object AssignedDriver { get; set; }
        public List<Shipment> ActiveShipments { get; set; }
    }
}