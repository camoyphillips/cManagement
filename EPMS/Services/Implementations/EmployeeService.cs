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
    // Service for working with employee data
    public class EmployeeService : IEmployeeService
    {
        private readonly EPMSDbContext dbContext;

        // Constructor that receives the database context
        public EmployeeService(EPMSDbContext context)
        {
            dbContext = context;
        }

        // Get all employees with their department names
        public async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
        {
            return await dbContext.Employees
                .Select(e => new EmployeeDTO
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department.DepartmentName
                })
                .ToListAsync();
        }

        // Get one employee with department and project info (for Details page)
        public async Task<EmployeeDTO> GetEmployeeDetailsAsync(int id)
        {
            var employee = await dbContext.Employees
                .Include(e => e.Department)
                .Include(e => e.EmployeeProjects)
                    .ThenInclude(ep => ep.Project)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null) return null;

            return new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.DepartmentName,
                ProjectNames = employee.EmployeeProjects?.Select(ep => ep.Project.ProjectName).ToList()
            };
        }

        // Get employee info for editing (no project info)
        public async Task<EmployeeCreateEditDTO> GetByIdForEditAsync(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null) return null;

            return new EmployeeCreateEditDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId
            };
        }

        // Add a new employee
        public async Task CreateAsync(EmployeeCreateEditDTO employeeDto)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                DepartmentId = employeeDto.DepartmentId
            };

            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync();
        }

        // Update an existing employee
        public async Task<bool> UpdateAsync(int id, EmployeeCreateEditDTO employeeDto)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null) return false;

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Email = employeeDto.Email;
            employee.DepartmentId = employeeDto.DepartmentId;

            await dbContext.SaveChangesAsync();
            return true;
        }

        // Delete an employee by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null) return false;

            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync();
            return true;
        }

        // Get one employee with department info (no project info)
        public async Task<EmployeeDTO> GetByIdAsync(int id)
        {
            var employee = await dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null) return null;

            return new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.DepartmentName
            };
        }
    }
}
