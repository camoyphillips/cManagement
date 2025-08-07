using cManagement.Data;
using cManagement.DTOs;
using cManagement.Models;
using cManagement.Services.Interfaces; // Assuming IDepartmentService is in cManagement.Services.Interfaces
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cManagement.Services.Implementations
{
    // Handles all operations related to Department data
    public class DepartmentService
: IDepartmentService
    {
        private readonly ApplicationDbContext _context; // Renamed _context to dbContext to match style

        // Constructor that receives the database context
        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all departments from the database
        public async Task<IEnumerable<DepartmentDTO>>
GetAllAsync()
        {
            return await
_context.Departments
                .Select(d => new DepartmentDTO
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                })
                .ToListAsync();
        }

        // Get one department by its ID
        public async Task<DepartmentDTO?>
GetByIdAsync(int id)
        {
            var dept =
await _context.Departments.FindAsync(id);
            if (dept
== null) return null;

            return new
DepartmentDTO
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName
            };
        }

        // Add a new department to the database
        public async Task<DepartmentDTO>
CreateAsync(DepartmentDTO departmentDto)
        {
            var dept =
new Department
            {
                DepartmentName = departmentDto.DepartmentName
            };

            _context.Departments.Add(dept);
            await
_context.SaveChangesAsync();

            // Set the new ID to return in the DTO
            departmentDto.DepartmentId = dept.DepartmentId;
            return
departmentDto;
        }

        // Update a department by its ID
        public async Task<bool>
UpdateAsync(int id, DepartmentDTO departmentDto)
        {
            var dept =
await _context.Departments.FindAsync(id);
            if (dept
== null) return false;

            dept.DepartmentName = departmentDto.DepartmentName;
            await
_context.SaveChangesAsync();
            return true;
        }

        // Delete a department by its ID
        public async Task<bool>
DeleteAsync(int id)
        {
            var dept =
await _context.Departments.FindAsync(id);
            if (dept
== null) return false;

            _context.Departments.Remove(dept);
            await
_context.SaveChangesAsync();
            return true;
        }
    }
}