using System;
using System.ComponentModel.DataAnnotations;

namespace cManagement.Models
{
    /// <summary>
    /// Join table between Driver and Shipment, with metadata about the assignment.
    /// </summary>
    public class DriverShipment
    {
        [Key]
        public int DriverShipmentId { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; } = null!;

        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; } = null!;

        [StringLength(50)]
        public string? Role { get; set; } 

        public DateTime AssignedOn { get; set; } = DateTime.UtcNow;
    }
}
