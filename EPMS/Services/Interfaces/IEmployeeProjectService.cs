using System.Collections.Generic;
using System.Threading.Tasks;
using EPMS.DTOs;

namespace EPMS.Services.Interfaces
{
    // Interface for working with employee-project assignments
    public interface IEmployeeProjectService
    {
        // Get all employee-project assignments
        Task<List<EmployeeProjectDTO>> GetAllAllocationsAsync();

        // Get one assignment by employee and project ID
        Task<EmployeeProjectDTO> GetAllocationAsync(int employeeId, int projectId);

        // Add a new employee-project assignment
        Task AddAllocationAsync(int employeeId, int projectId);

        // Update an assignment with new employee and project IDs
        Task UpdateAllocationAsync(int oldEmployeeId, int oldProjectId, int newEmployeeId, int newProjectId);

        // Delete an assignment
        Task RemoveAllocationAsync(int employeeId, int projectId);
    }
}
