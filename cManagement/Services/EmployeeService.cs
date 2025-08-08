using System.Security.Claims;
using cManagement.Data;
using cManagement.DTOs;
using cManagement.Models;
using cManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace cManagement.Services.Implementations
{
    /// <summary>
    /// Employee service with Identity-backed authorization.
    /// Roles: "Admin" (full access) and "Employee" (own record only).
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _http;

        private const string RoleAdmin = "Admin";
        private const string RoleEmployee = "Employee";

        public EmployeeService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _http = httpContextAccessor;
        }

        #region Helpers

        private ClaimsPrincipal? CurrentPrincipal => _http.HttpContext?.User;

        private Task<ApplicationUser?> GetCurrentUserAsync()
            => _userManager.GetUserAsync(CurrentPrincipal!);

        private async Task<bool> IsInRoleAsync(string role)
        {
            var user = await GetCurrentUserAsync();
            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        private Task<bool> IsAdminAsync() => IsInRoleAsync(RoleAdmin);
        private Task<bool> IsEmployeeAsync() => IsInRoleAsync(RoleEmployee);

        #endregion

        public async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
        {
            if (await IsAdminAsync())
            {
                return await _context.Employees
                    .AsNoTracking()
                    .Include(e => e.Department)
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

            if (await IsEmployeeAsync())
            {
                var me = await GetCurrentUserAsync();
                if (me == null) return Enumerable.Empty<EmployeeDTO>();

                var myRecord = await _context.Employees
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
                        DepartmentName = e.Department.DepartmentName
                    })
                    .ToListAsync();

                return myRecord;
            }

            return Enumerable.Empty<EmployeeDTO>();
        }

        public async Task<EmployeeDTO?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null) return null;

            if (await IsAdminAsync())
            {
                return MapToDTO(employee);
            }

            if (await IsEmployeeAsync())
            {
                var me = await GetCurrentUserAsync();
                if (me != null && employee.ApplicationUserId == me.Id)
                {
                    return MapToDTO(employee);
                }
                return null;
            }

            return null;
        }

        public async Task<EmployeeDTO?> GetEmployeeDetailsAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.EmployeeProjects)
                    .ThenInclude(ep => ep.Project)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null) return null;

            if (await IsAdminAsync())
            {
                return MapToDTOWithProjects(employee);
            }

            if (await IsEmployeeAsync())
            {
                var me = await GetCurrentUserAsync();
                if (me != null && employee.ApplicationUserId == me.Id)
                {
                    return MapToDTOWithProjects(employee);
                }
                return null;
            }

            return null;
        }

        public async Task<ServiceResponse> CreateAsync(EmployeeCreateEditDTO dto)
        {
            if (!await IsAdminAsync())
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.Error,
                    Messages = new List<string> { "Forbidden: You do not have permission to create employees." }
                };
            }

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId
                // ApplicationUserId = dto.ApplicationUserId; // if linking
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = employee.EmployeeId
            };
        }

        public async Task<ServiceResponse> UpdateAsync(int id, EmployeeCreateEditDTO dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };

            if (!await IsAdminAsync())
            {
                if (await IsEmployeeAsync())
                {
                    var me = await GetCurrentUserAsync();
                    if (me == null || employee.ApplicationUserId != me.Id)
                        return new ServiceResponse
                        {
                            Status = ServiceResponse.ServiceStatus.Error,
                            Messages = new List<string> { "Forbidden: You cannot update this employee." }
                        };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Status = ServiceResponse.ServiceStatus.Error,
                        Messages = new List<string> { "Forbidden: You cannot update this employee." }
                    };
                }
            }

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.DepartmentId = dto.DepartmentId;

            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Updated };
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };

            if (!await IsAdminAsync())
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.Error,
                    Messages = new List<string> { "Forbidden: You cannot delete this employee." }
                };
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
        }

        public async Task<EmployeeDTO?> Profile()
        {
            if (!await IsEmployeeAsync()) return null;

            var me = await GetCurrentUserAsync();
            if (me == null) return null;

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.ApplicationUserId == me.Id);

            return employee == null ? null : MapToDTO(employee);
        }

        #region Private Mappers

        private static EmployeeDTO MapToDTO(Employee e) =>
            new EmployeeDTO
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department?.DepartmentName
            };

        private static EmployeeDTO MapToDTOWithProjects(Employee e) =>
            new EmployeeDTO
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department?.DepartmentName,
                ProjectNames = e.EmployeeProjects?
                    .Select(ep => ep.Project.ProjectName)
                    .ToList()
            };

        public Task<EmployeeCreateEditDTO> GetByIdForEditAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IEmployeeService.CreateAsync(EmployeeCreateEditDTO employeeDto)
        {
            return CreateAsync(employeeDto);
        }

        Task<bool> IEmployeeService.UpdateAsync(int id, EmployeeCreateEditDTO employeeDto)
        {
            throw new NotImplementedException();
        }

        Task<bool> IEmployeeService.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
