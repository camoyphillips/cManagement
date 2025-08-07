using cManagement.Data;
using cManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cManagement.Services.Implementations
{
    // This DbContext is likely meant to mirror ApplicationDbContext — you can rename or merge it if needed
    public class cManagementDbContext : IdentityDbContext<ApplicationUser>
    {
        public cManagementDbContext(DbContextOptions<cManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<EmployeeShipment> EmployeeShipments { get; set; }
        public DbSet<DriverShipment> DriverShipments { get; set; }
    }
}
