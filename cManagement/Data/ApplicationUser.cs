using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using cManagement.Models;

namespace cManagement.Data
{
    /// <summary>
    /// Custom identity user for ASP.NET Core Identity.
    /// Includes inverse navigations to domain entities that reference ApplicationUserId.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Employees that are owned/created by this user (FK on Employee.ApplicationUserId).
        /// </summary>
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

        /// <summary>
        /// Drivers that are owned/created by this user (FK on Driver.ApplicationUserId).
        /// </summary>
        public ICollection<Driver> Drivers { get; set; } = new HashSet<Driver>();

        /// <summary>
        /// Shipments that are owned/created by this user (FK on Shipment.ApplicationUserId).
        /// </summary>
        public ICollection<Shipment> Shipments { get; set; } = new HashSet<Shipment>();
    }
}
