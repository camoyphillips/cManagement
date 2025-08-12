using System;
using System.ComponentModel.DataAnnotations;

namespace cManagement.Models
{
    /// <summary>
    /// Driver ↔ Shipment assignment with metadata about the assignment.
    /// Your table includes DriverShipmentId as a separate key (not identity).
    /// </summary>
    public class DriverShipment
    {
        [Key]
        public int DriverShipmentId { get; set; }  // ValueGeneratedNever() in DbContext

        public int DriverId { get; set; }
        public Driver Driver { get; set; } = null!;

        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; } = null!;

        [StringLength(50)]
        public string? Role { get; set; }

        public DateTime AssignedOn { get; set; } = DateTime.UtcNow;
    }
}
