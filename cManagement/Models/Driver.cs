using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cManagement.Models
{
    /// <summary>
    /// Represents a Driver in the system, assigned to Shipments via many-to-many.
    /// </summary>
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Phone, StringLength(20)]
        public string? ContactNumber { get; set; }

        /// <summary>
        /// Navigation to all Driver-Shipments.
        /// </summary>
        public ICollection<DriverShipment> DriverShipments { get; set; } = new List<DriverShipment>();
    }
}
