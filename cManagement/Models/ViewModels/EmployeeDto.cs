using System;
using System.ComponentModel.DataAnnotations;

namespace cManagement.ViewModels
{
    /// <summary>
    /// Lightweight view model for employee data used by API/controllers.
    /// </summary>
    public class EmployeeDto
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Display name for the employee (use a single field even if your model stores First/Last).
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Optional email address.
        /// </summary>
        [EmailAddress, StringLength(256)]
        public string? Email { get; set; }

        /// <summary>
        /// Optional phone/contact number.
        /// </summary>
        [Phone, StringLength(30)]
        public string? ContactNumber { get; set; }

        /// <summary>
        /// Job title / role.
        /// </summary>
        [StringLength(100)]
        public string? Title { get; set; }

        /// <summary>
        /// Employment status flag (set when projecting from the entity).
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date of hire (nullable if unknown).
        /// </summary>
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// Foreign key to Department (nullable if unassigned).
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Convenience property for displaying the department name in lists.
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Required link to AspNetUsers (Identity) owner/creator.
        /// </summary>
        [Required]
        public string ApplicationUserId { get; set; } = default!;
    }
}
