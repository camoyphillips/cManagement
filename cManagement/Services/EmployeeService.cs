using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cManagement.Data;
using cManagement.DTOs;
using cManagement.Models;
// using cManagement.Services.Implementations; // ❌ remove: no cManagementDbContext
using cManagement.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace cManagement.Services
{
    /// <summary>
    /// Employee service with Identity-backed authorization.
    /// Roles: "Admin" (full access) and "Employee" (own record only).
    /// Implements IEmployeeService signatures exactly.
    /// </summary>
    public class EmployeeService(
        ApplicationDbContext context,                // ✅ use ApplicationDbContext
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor http) : IEmployeeService
    {
        private const string RoleAdmin = "Admin";
        private const string RoleEmployee = "Employee";

        #region Helpers

        private ClaimsPrincipal? CurrentPrincipal => http.HttpContext?.User;

        private Task<ApplicationUser?> GetCurrentUserAsync() =>
            CurrentPrincipal != null ? userManager.GetUserAsync(CurrentPrincipal) : Task.FromResult<ApplicationUser?>(null);

        private async Task<bool> IsInRoleAsync(string role)
        {
            var user = await GetCurrentUserAsync();
            return user != null && await userManager.IsInRoleAsync(user, role);
        }

        private Task<bool> IsAdminAsync() => IsInRoleAsync(RoleAdmin);
        private Task<bool> IsEmployeeAsync() => IsInRoleAsync(RoleEmployee);

        private static EmployeeCreateEditDTO MapToEditDTO(Employee e) =>
            new EmployeeCreateEditDTO
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                DepartmentId = e.DepartmentId
            };

        /// <summary>Ensure an Identity user exists for the given email and return it.</summary>
        private async Task<ApplicationUser> EnsureIdentityUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null) return user;

            user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                EmailConfirmed = true
            };

            var tempPassword = "Aa1!" + Guid.NewGuid().ToString("N") + "z!";
            var result = await userManager.CreateAsync(user, tempPassword);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            // Optionally: await userManager.AddToRoleAsync(user, RoleEmployee);
            return user;
        }

        private static void EnsureFound(object? entity, string message)
        {
            if (entity is null) throw new KeyNotFoundException(message);
        }

        private async Task EnsureCanReadAsync(Employee employee)
        {
            if (await IsAdminAsync()) return;

            if (await IsEmployeeAsync())
            {
                var me = await GetCurrentUserAsync();
                if (me != null && employee.ApplicationUserId == me.Id) return;
            }

            throw new UnauthorizedAccessException("You are not allowed to view this employee.");
        }

        private async Task EnsureCanModifyAsync(Employee employee)
        {
            if (await IsAdminAsync()) return;

            if (await IsEmployeeAsync())
            {
                var me = await GetCurrentUserAsync();
                if (me != null && employee.ApplicationUserId == me.Id) return;
            }

            throw new UnauthorizedAccessException("You are not allowed to modify this employee.");
        }

        #endregion

        // ─────────────────────────────────────────────────────────────
        // IEmployeeService implementation (exact signatures)
        // ─────────────────────────────────────────────────────────────

        public async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
        {
            if (await IsAdminAsync())
            {
                return await context.Employees
                    .AsNoTracking()
                    .Include(e => e.Department)
                    .Select(e => new EmployeeDTO
                    {
                        EmployeeId = e.EmployeeId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Email = e.Email,
                        DepartmentId = e.DepartmentId,
                        // avoid null-propagation in expression tree
                        DepartmentName = e.Department == null ? string.Empty : e.Department.DepartmentName
                    })
                    .ToListAsync();
            }

            if (await IsEmployeeAsync())
            {
                var me = await GetCurrentUserAsync();
                if (me == null) return Enumerable.Empty<EmployeeDTO>();

                return await context.Employees
                    .AsNoTracking()
                    .Include(e => e.Department)
                    .Where(e => e.ApplicationUserId == me.Id)
                    .Select(e => new EmployeeDTO
                    {
                        EmployeeId = e.EmployeeId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Email = e.Email,
                        DepartmentId = e.DepartmentId,
                        DepartmentName = e.Department == null ? string.Empty : e.Department.DepartmentName
                    })
                    .ToListAsync();
            }

            return Enumerable.Empty<EmployeeDTO>();
        }

        public async Task<EmployeeDTO> GetByIdAsync(int id)
        {
            var employee = await context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            EnsureFound(employee, $"Employee with id {id} was not found.");
            await EnsureCanReadAsync(employee!);

            return new EmployeeDTO
            {
                EmployeeId = employee!.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department == null ? string.Empty : employee.Department.DepartmentName
            };
        }

        public async Task<EmployeeDTO> GetEmployeeDetailsAsync(int id)
        {
            var employee = await context.Employees
                .Include(e => e.Department)
                .Include(e => e.EmployeeProjects)
                    .ThenInclude(ep => ep.Project)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            EnsureFound(employee, $"Employee with id {id} was not found.");
            await EnsureCanReadAsync(employee!);

            return new EmployeeDTO
            {
                EmployeeId = employee!.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department == null ? string.Empty : employee.Department.DepartmentName,
                ProjectNames = (employee.EmployeeProjects != null
                                ? employee.EmployeeProjects.Where(ep => ep.Project != null).Select(ep => ep.Project!.ProjectName)
                                : Enumerable.Empty<string>())
                               .ToList()
            };
        }

        public async Task<EmployeeCreateEditDTO> GetByIdForEditAsync(int id)
        {
            var employee = await context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            EnsureFound(employee, $"Employee with id {id} was not found.");
            await EnsureCanReadAsync(employee!);

            return MapToEditDTO(employee!);
        }

        public async Task CreateAsync(EmployeeCreateEditDTO dto)
        {
            if (!await IsAdminAsync())
                throw new UnauthorizedAccessException("You do not have permission to create employees.");

            var user = await EnsureIdentityUserAsync(dto.Email);

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId,
                ApplicationUserId = user.Id
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, EmployeeCreateEditDTO dto)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return false;

            await EnsureCanModifyAsync(employee);

            // If email changes, update Identity user too
            if (!string.Equals(employee.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var identityUser = await userManager.FindByIdAsync(employee.ApplicationUserId);
                if (identityUser != null)
                {
                    identityUser.Email = dto.Email;
                    identityUser.UserName = dto.Email;
                    var ures = await userManager.UpdateAsync(identityUser);
                    if (!ures.Succeeded)
                        throw new InvalidOperationException("Failed to update Identity user: " + string.Join("; ", ures.Errors.Select(e => e.Description)));
                }
                employee.Email = dto.Email;
            }

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.DepartmentId = dto.DepartmentId;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return false;

            if (!await IsAdminAsync())
                throw new UnauthorizedAccessException("You do not have permission to delete employees.");

            context.Employees.Remove(employee);
            await context.SaveChangesAsync();

            // also delete/disable the Identity user
            // var user = await userManager.FindByIdAsync(employee.ApplicationUserId);
            // if (user != null) await userManager.DeleteAsync(user);

            return true;
        }

        // Convenience (not required by interface)
        public async Task<EmployeeDTO?> Profile()
        {
            if (!await IsEmployeeAsync()) return null;

            var me = await GetCurrentUserAsync();
            if (me == null) return null;

            var employee = await context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.ApplicationUserId == me.Id);

            return employee == null
                ? null
                : new EmployeeDTO
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    DepartmentId = employee.DepartmentId,
                    DepartmentName = employee.Department == null ? string.Empty : employee.Department.DepartmentName
                };
        }
    }
}
