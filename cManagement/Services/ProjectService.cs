using cManagement.Data;
using cManagement.DTOs;
using cManagement.Models;
using cManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cManagement.Services.Implementations
{
    // Service for working with project data
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        // Constructor that receives the database context
        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all projects
        public async Task<IEnumerable<ProjectDTO>> GetAllAsync()
        {
            return await _context.Projects
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
            var proj = await _context.Projects.FindAsync(id);
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

            _context.Projects.Add(proj);
            await _context.SaveChangesAsync();

            // Save the new ID to the DTO
            projectDto.ProjectId = proj.ProjectId;
            return projectDto;
        }

        // Update an existing project
        public async Task<bool> UpdateAsync(int id, ProjectDTO projectDto)
        {
            var proj = await _context.Projects.FindAsync(id);
            if (proj == null) return false;

            proj.ProjectName = projectDto.ProjectName;
            proj.Description = projectDto.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a project by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var proj = await _context.Projects.FindAsync(id);
            if (proj == null) return false;

            _context.Projects.Remove(proj);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
