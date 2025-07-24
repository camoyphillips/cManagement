using EPMS.Data;
using EPMS.DTOs;
using EPMS.Models;
using EPMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPMS.Services.Implementations
{
    // Service for working with project data
    public class ProjectService : IProjectService
    {
        private readonly EPMSDbContext dbContext;

        // Constructor that receives the database context
        public ProjectService(EPMSDbContext context)
        {
            dbContext = context;
        }

        // Get all projects
        public async Task<IEnumerable<ProjectDTO>> GetAllAsync()
        {
            return await dbContext.Projects
                .Select(p => new ProjectDTO
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Description = p.Description
                })
                .ToListAsync();
        }

        // Get one project by ID
        public async Task<ProjectDTO?> GetByIdAsync(int id)
        {
            var proj = await dbContext.Projects.FindAsync(id);
            if (proj == null) return null;

            return new ProjectDTO
            {
                ProjectId = proj.ProjectId,
                ProjectName = proj.ProjectName,
                Description = proj.Description
            };
        }

        // Add a new project
        public async Task<ProjectDTO> CreateAsync(ProjectDTO projectDto)
        {
            var proj = new Project
            {
                ProjectName = projectDto.ProjectName,
                Description = projectDto.Description
            };

            dbContext.Projects.Add(proj);
            await dbContext.SaveChangesAsync();

            // Save the new ID to the DTO
            projectDto.ProjectId = proj.ProjectId;
            return projectDto;
        }

        // Update an existing project
        public async Task<bool> UpdateAsync(int id, ProjectDTO projectDto)
        {
            var proj = await dbContext.Projects.FindAsync(id);
            if (proj == null) return false;

            proj.ProjectName = projectDto.ProjectName;
            proj.Description = projectDto.Description;

            await dbContext.SaveChangesAsync();
            return true;
        }

        // Delete a project by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var proj = await dbContext.Projects.FindAsync(id);
            if (proj == null) return false;

            dbContext.Projects.Remove(proj);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
