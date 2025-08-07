using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using cManagement.Models;

namespace cManagement.Data
{
    /// <summary>
    /// EF Core DbContext configured for ASP.NET Identity and cManagement entities.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Truck> Trucks { get; set; } = null!;
        public DbSet<Shipment> Shipments { get; set; } = null!;
        public DbSet<DriverShipment> DriverShipments { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<EmployeeProject> EmployeeProjects { get; set; } = null!;
        public DbSet<EmployeeShipment> EmployeeShipments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Composite key and relationships for EmployeeProject
            modelBuilder.Entity<EmployeeProject>()
                .HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeeProjects)
                .HasForeignKey(ep => ep.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            //Composite key and relationships for DriverShipment
            modelBuilder.Entity<DriverShipment>()
                .HasKey(ds => new { ds.DriverId, ds.ShipmentId });

            modelBuilder.Entity<DriverShipment>()
                .HasOne(ds => ds.Driver)
                .WithMany(d => d.DriverShipments)
                .HasForeignKey(ds => ds.DriverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DriverShipment>()
                .HasOne(ds => ds.Shipment)
                .WithMany(s => s.DriverShipments)
                .HasForeignKey(ds => ds.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Composite key and relationships for EmployeeShipment
            modelBuilder.Entity<EmployeeShipment>()
                .HasKey(es => new { es.EmployeeId, es.ShipmentId });

            modelBuilder.Entity<EmployeeShipment>()
                .HasOne(es => es.Employee)
                .WithMany(e => (IEnumerable<EmployeeShipment>)e.EmployeeShipments)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeShipment>()
                .HasOne(es => es.Shipment)
                .WithMany(s => s.EmployeeShipments)
                .HasForeignKey(es => es.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
