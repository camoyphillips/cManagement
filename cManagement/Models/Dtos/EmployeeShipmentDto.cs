using System;

namespace cManagement.Models
{
    /// <summary>
    /// Data Transfer Object for EmployeeShipment.
    /// </summary>
    public class EmployeeShipmentDto
    {
        /// <summary>
        /// The unique identifier for the EmployeeShipment record. Nullable for new records.
        /// </summary>
        public int? EmployeeShipmentId { get; set; }

        /// <summary>
        /// The ID of the employee.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// The ID of the shipment.
        /// </summary>
        public int ShipmentId { get; set; }

        /// <summary>
        /// The role of the employee in the shipment (e.g., Loader, Supervisor).
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// The current status of the employee's task in the shipment.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Timestamp when the employee completed their task.
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Timestamp when the employee was assigned to the shipment.
        /// </summary>
        public DateTime AssignedOn { get; set; }
    }
}