using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cManagement.Data;

namespace cManagement.Models
{
    /// <summary>
    /// Represents a Driver in the system, assigned to Shipments via many-to-many relationship.
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

        /// <summary>Optional FK to a truck the driver is currently assigned to.</summary>
        public int? AssignedTruckId { get; set; }

        [ForeignKey(nameof(AssignedTruckId))]
        public Truck? AssignedTruck { get; set; }

        /// <summary>Required link to AspNetUsers.Id (nvarchar).</summary>
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        public ApplicationUser? ApplicationUser { get; set; }

        /// <summary>Navigation to all associated Driver-Shipments.</summary>
        public ICollection<DriverShipment> DriverShipments { get; set; } = new List<DriverShipment>();
    }
}
