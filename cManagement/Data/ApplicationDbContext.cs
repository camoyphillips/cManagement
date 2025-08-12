using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using cManagement.Data;
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

            // Employee  Identity user (AspNetUsers)
            modelBuilder.Entity<Employee>()
                .Property(e => e.ApplicationUserId)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ApplicationUser)
                .WithMany()
                .HasForeignKey(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver  Identity user (AspNetUsers)
            modelBuilder.Entity<Driver>()
                .Property(d => d.ApplicationUserId)
                .IsRequired();

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.ApplicationUser)
                .WithMany()
                .HasForeignKey(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver  optional Truck
            modelBuilder.Entity<Driver>()
                .HasOne(d => d.AssignedTruck)
                .WithMany()
                .HasForeignKey(d => d.AssignedTruckId)
                .OnDelete(DeleteBehavior.SetNull);

            // Shipment  Truck (required)
            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Truck)
                .WithMany()
                .HasForeignKey(s => s.TruckId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department  Employees (1-M)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // EmployeeProject (many-to-many via explicit join)
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

            // EmployeeShipment (composite PK per your table)
            modelBuilder.Entity<EmployeeShipment>()
                .HasKey(es => new { es.EmployeeId, es.ShipmentId });

            modelBuilder.Entity<EmployeeShipment>()
                .Property(es => es.Status)
                .HasConversion<int>(); 

            modelBuilder.Entity<EmployeeShipment>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeShipments)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeShipment>()
                .HasOne(es => es.Shipment)
                .WithMany(s => s.EmployeeShipments)
                .HasForeignKey(es => es.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // DriverShipment (separate PK column DriverShipmentId)
            modelBuilder.Entity<DriverShipment>()
                .HasKey(ds => ds.DriverShipmentId);

            modelBuilder.Entity<DriverShipment>()
                .Property(ds => ds.DriverShipmentId)
                .ValueGeneratedNever();

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
        }
    }
}
