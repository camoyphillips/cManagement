using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cManagement.Models
{
    /// <summary>
    /// Represents a Truck that handles Shipments and may be linked to a Driver.
    /// </summary>
    public class Truck
    {
        [Key]
        public int TruckId { get; set; }

        [Required]
        public string Model { get; set; } = string.Empty;

        public int Mileage { get; set; }
        public DateTime LastMaintenanceDate { get; set; }

        public string? TruckImagePath { get; set; }

        public int? AssignedDriverId { get; set; }
        public Driver? AssignedDriver { get; set; }

        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
