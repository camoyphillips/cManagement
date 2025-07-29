using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cManagement.Models
{
    /// <summary>
    /// Represents a Shipment in the logistics platform.
    /// </summary>
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }

        [Required, StringLength(100)]
        public string Origin { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public int Distance { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = "Pending";

        [ForeignKey("Truck")]
        public int TruckId { get; set; }
        public Truck Truck { get; set; } = null!;

        public ICollection<DriverShipment> DriverShipments { get; set; } = new List<DriverShipment>();
        public ICollection<EmployeeShipment> EmployeeShipments { get; set; } = new List<EmployeeShipment>();
    }
}
