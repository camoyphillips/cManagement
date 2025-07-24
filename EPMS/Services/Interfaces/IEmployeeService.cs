using EPMS.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

// Interface for working with employee data
public interface IEmployeeService
{
    // Get all employees
    Task<IEnumerable<EmployeeDTO>> GetAllAsync();

    // Get one employee by ID
    Task<EmployeeDTO> GetByIdAsync(int id);

    // Get employee data for editing
    Task<EmployeeCreateEditDTO> GetByIdForEditAsync(int id);

    // Get full employee details (includes department and projects)
    Task<EmployeeDTO> GetEmployeeDetailsAsync(int id);

    // Add a new employee
    Task CreateAsync(EmployeeCreateEditDTO employeeDto);

    // Update an existing employee
    Task<bool> UpdateAsync(int id, EmployeeCreateEditDTO employeeDto);

    // Delete an employee
    Task<bool> DeleteAsync(int id);
}
