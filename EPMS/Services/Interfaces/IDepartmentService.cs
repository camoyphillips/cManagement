using EPMS.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Services.Interfaces
{
    // Interface for working with departments
    public interface IDepartmentService
    {
        // Get all departments
        Task<IEnumerable<DepartmentDTO>> GetAllAsync();

        // Get one department by ID
        Task<DepartmentDTO?> GetByIdAsync(int id);

        // Add a new department
        Task<DepartmentDTO> CreateAsync(DepartmentDTO departmentDto);

        // Update a department
        Task<bool> UpdateAsync(int id, DepartmentDTO departmentDto);

        // Delete a department
        Task<bool> DeleteAsync(int id);
    }
}
