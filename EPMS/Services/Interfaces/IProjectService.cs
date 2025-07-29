using EPMS.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Services.Interfaces
{
    // Interface for working with project data
    public interface IProjectService
    {
        // Get all projects
        Task<IEnumerable<ProjectDTO>> GetAllAsync();

        // Get one project by ID
        Task<ProjectDTO?> GetByIdAsync(int id);

        // Add a new project
        Task<ProjectDTO> CreateAsync(ProjectDTO projectDto);

        // Update a project
        Task<bool> UpdateAsync(int id, ProjectDTO projectDto);

        // Delete a project
        Task<bool> DeleteAsync(int id);
    }
}
