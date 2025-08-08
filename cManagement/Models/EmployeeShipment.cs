using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cManagement.Models
{
    /// <summary>
    /// Join table representing a many-to-many relationship between Employees and Shipments.
    /// Includes task status, role, and assignment tracking.
    /// </summary>
    public class EmployeeShipment
    {
        /// <summary>
        /// Foreign key to the Employee.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Navigation property to the Employee.
        /// </summary>
        public Employee Employee { get; set; } = null!;

        /// <summary>
        /// Foreign key to the Shipment.
        /// </summary>
        public int ShipmentId { get; set; }

        /// <summary>
        /// Navigation property to the Shipment.
        /// </summary>
        public Shipment Shipment { get; set; } = null!;

        /// <summary>
        /// The role of the employee in the context of this shipment (e.g., Loader, Supervisor).
        /// </summary>
        [MaxLength(50)]
        public string? Role { get; set; }

        /// <summary>
        /// The current status of the employee's task in the shipment.
        /// </summary>
        public TaskStatus Status { get; set; } = TaskStatus.Assigned;

        /// <summary>
        /// Timestamp of when the employee completed their task.
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Timestamp of when the employee was assigned to the shipment.
        /// </summary>
        public DateTime AssignedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Primary key for the EmployeeShipment join entity.
        /// </summary>
        public int EmployeeShipmentId { get; internal set; }
    }

    /// <summary>
    /// Enum for task progress tracking.
    /// </summary>
    public enum TaskStatus
    {
        Assigned,
        InProgress,
        Completed
    }
}