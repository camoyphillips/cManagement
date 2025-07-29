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
    // Handles all operations related to Department data
    public class DepartmentService : IDepartmentService
    {
        private readonly EPMSDbContext dbContext;

        // Constructor that receives the database context
        public DepartmentService(EPMSDbContext context)
        {
            dbContext = context;
        }

        // Get all departments from the database
        public async Task<IEnumerable<DepartmentDTO>> GetAllAsync()
        {
            return await dbContext.Departments
                .Select(d => new DepartmentDTO
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                })
                .ToListAsync();
        }

        // Get one department by its ID
        public async Task<DepartmentDTO?> GetByIdAsync(int id)
        {
            var dept = await dbContext.Departments.FindAsync(id);
            if (dept == null) return null;

            return new DepartmentDTO
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName
            };
        }

        // Add a new department to the database
        public async Task<DepartmentDTO> CreateAsync(DepartmentDTO departmentDto)
        {
            var dept = new Department
            {
                DepartmentName = departmentDto.DepartmentName
            };

            dbContext.Departments.Add(dept);
            await dbContext.SaveChangesAsync();

            // Set the new ID to return in the DTO
            departmentDto.DepartmentId = dept.DepartmentId;
            return departmentDto;
        }

        // Update a department by its ID
        public async Task<bool> UpdateAsync(int id, DepartmentDTO departmentDto)
        {
            var dept = await dbContext.Departments.FindAsync(id);
            if (dept == null) return false;

            dept.DepartmentName = departmentDto.DepartmentName;
            await dbContext.SaveChangesAsync();
            return true;
        }

        // Delete a department by its ID
        public async Task<bool> DeleteAsync(int id)
        {
            var dept = await dbContext.Departments.FindAsync(id);
            if (dept == null) return false;

            dbContext.Departments.Remove(dept);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
