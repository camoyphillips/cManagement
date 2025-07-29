using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPMS.DTOs;
using EPMS.Models;
using EPMS.Data; 
using Microsoft.EntityFrameworkCore;
using EPMS.Services.Interfaces;

namespace EPMS.Services.Implementations
{
    // Handles assigning projects to employees (many-to-many relationship)
    public class EmployeeProjectService : IEmployeeProjectService
    {
        private readonly EPMSDbContext dbContext;

        // Constructor that receives the database context
        public EmployeeProjectService(EPMSDbContext context)
        {
            dbContext = context;
        }

        // Get all employee-project assignments with employee and project names
        public async Task<List<EmployeeProjectDTO>> GetAllAllocationsAsync()
        {
            var allocations = await dbContext.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project)
                .Select(ep => new EmployeeProjectDTO
                {
                    EmployeeId = ep.EmployeeId,
                    ProjectId = ep.ProjectId,
                    FirstName = ep.Employee.FirstName,
                    LastName = ep.Employee.LastName,
                    ProjectName = ep.Project.ProjectName
                })
                .ToListAsync();

            return allocations;
        }

        // Get one assignment by employee and project ID
        public async Task<EmployeeProjectDTO> GetAllocationAsync(int employeeId, int projectId)
        {
            var allocation = await dbContext.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project)
                .Where(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId)
                .Select(ep => new EmployeeProjectDTO
                {
                    EmployeeId = ep.EmployeeId,
                    ProjectId = ep.ProjectId,
                    FirstName = ep.Employee.FirstName,
                    LastName = ep.Employee.LastName,
                    ProjectName = ep.Project.ProjectName
                })
                .FirstOrDefaultAsync();

            return allocation;
        }

        // Add a new assignment between an employee and a project
        public async Task AddAllocationAsync(int employeeId, int projectId)
        {
            var exists = await dbContext.EmployeeProjects
                .AnyAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

            if (!exists)
            {
                var allocation = new EmployeeProject
                {
                    EmployeeId = employeeId,
                    ProjectId = projectId
                };

                dbContext.EmployeeProjects.Add(allocation);
                await dbContext.SaveChangesAsync();
            }
        }

        // Update an assignment (remove old, add new)
        public async Task UpdateAllocationAsync(int oldEmployeeId, int oldProjectId, int newEmployeeId, int newProjectId)
        {
            var oldAllocation = await dbContext.EmployeeProjects
                .FirstOrDefaultAsync(ep => ep.EmployeeId == oldEmployeeId && ep.ProjectId == oldProjectId);

            if (oldAllocation != null)
            {
                dbContext.EmployeeProjects.Remove(oldAllocation);

                var newAllocation = new EmployeeProject
                {
                    EmployeeId = newEmployeeId,
                    ProjectId = newProjectId
                };

                dbContext.EmployeeProjects.Add(newAllocation);
                await dbContext.SaveChangesAsync();
            }
        }

        // Remove an assignment between employee and project
        public async Task RemoveAllocationAsync(int employeeId, int projectId)
        {
            var allocation = await dbContext.EmployeeProjects
                .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

            if (allocation != null)
            {
                dbContext.EmployeeProjects.Remove(allocation);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
