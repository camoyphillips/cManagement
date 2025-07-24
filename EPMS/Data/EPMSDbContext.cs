using Microsoft.EntityFrameworkCore;
using EPMS.Models;

namespace EPMS.Data
{
    // This is the main database context
    public class EPMSDbContext : DbContext
    {
        // Constructor used to set up the context
        public EPMSDbContext(DbContextOptions<EPMSDbContext> options) : base(options)
        {
        }

        // Table for departments
        public DbSet<Department> Departments { get; set; }

        // Table for employees
        public DbSet<Employee> Employees { get; set; }

        // Table for projects
        public DbSet<Project> Projects { get; set; }

        // Table for employee-project allocations (many-to-many)
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }

        // Set up keys and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for employee-project join table
            modelBuilder.Entity<EmployeeProject>()
                .HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

            // One employee can have many project links
            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeeProjects)
                .HasForeignKey(ep => ep.EmployeeId);

            // One project can have many employee links
            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.ProjectId);
        }
    }
}
