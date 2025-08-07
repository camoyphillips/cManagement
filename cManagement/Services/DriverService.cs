using System.Security.Claims;
using cManagement.Data;
using cManagement.Interfaces;
using cManagement.Models;
using cManagement.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace cManagement.Services
{
    /// <summary>
    /// Driver service that supports Identity-backed authorization.
    /// Roles used: "Admin" (full access) and "Driver" (own record only).
    /// </summary>
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _http;

        private const string RoleAdmin = "Admin";
        private const string RoleDriver = "Driver";

        public DriverService(
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

        private async Task<bool> IsAdminAsync() => await IsInRoleAsync(RoleAdmin);
        private async Task<bool> IsDriverAsync() => await IsInRoleAsync(RoleDriver);

        #endregion

        public async Task<IEnumerable<DriverDto>> ListDrivers()
        {
            // Admins get all drivers. Drivers only get their own (if FK present).
            if (await IsAdminAsync())
            {
                return await _context.Drivers
                    .AsNoTracking()
                    .Select(d => new DriverDto
                    {
                        DriverId = d.DriverId,
                        Name = d.Name,
                        LicenseNumber = d.LicenseNumber
                    })
                    .ToListAsync();
            }

            if (await IsDriverAsync())
            {
                var me = await GetCurrentUserAsync();
                if (me == null) return Enumerable.Empty<DriverDto>();

                // If your Driver model does NOT have ApplicationUserId, either add it,
                // or replace this with your own mapping logic (e.g., by email).
                var mine = await _context.Drivers
                    .AsNoTracking()
                    .Where(d => d.ApplicationUserId == me.Id) // <-- requires FK
                    .Select(d => new DriverDto
                    {
                        DriverId = d.DriverId,
                        Name = d.Name,
                        LicenseNumber = d.LicenseNumber
                    })
                    .ToListAsync();

                return mine;
            }

            // Unprivileged users get nothing.
            return Enumerable.Empty<DriverDto>();
        }

        public async Task<DriverDto?> FindDriver(int id)
        {
            var driver = await _context.Drivers.AsNoTracking().FirstOrDefaultAsync(d => d.DriverId == id);
            if (driver == null) return null;

            if (await IsAdminAsync())
            {
                return new DriverDto
                {
                    DriverId = driver.DriverId,
                    Name = driver.Name,
                    LicenseNumber = driver.LicenseNumber
                };
            }

            if (await IsDriverAsync())
            {
                var me = await GetCurrentUserAsync();
                // Only allow if the record belongs to the current driver
                if (me != null && driver.ApplicationUserId == me.Id)
                {
                    return new DriverDto
                    {
                        DriverId = driver.DriverId,
                        Name = driver.Name,
                        LicenseNumber = driver.LicenseNumber
                    };
                }

                // Hide existence of others' records
                return null;
            }

            // No access
            return null;
        }

        public async Task<ServiceResponse> AddDriver(DriverDto dto)
        {
            if (!await IsAdminAsync())
            {
               
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Error, Messages = new List<string> { "Forbidden: You do not have permission to perform this action." } };
            }

            var driver = new Driver
            {
                Name = dto.Name,
                LicenseNumber = dto.LicenseNumber,
                // ApplicationUserId = dto.ApplicationUserId
            };

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = driver.DriverId
            };
        }

        public async Task<ServiceResponse> UpdateDriver(DriverDto dto)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverId == dto.DriverId);
            if (driver == null) return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };

            // Admin can update any; Driver can only update their own
            if (!await IsAdminAsync())
            {
                if (await IsDriverAsync())
                {
                    var me = await GetCurrentUserAsync();
                    if (me == null || driver.ApplicationUserId != me.Id)
                        return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Error, Messages = new List<string> { "Forbidden: You do not have permission to update this driver." } };
                }
                else
                {
                    return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Error, Messages = new List<string> { "Forbidden: You do not have permission to update this driver." } };
                }
            }

            driver.Name = dto.Name;
            driver.LicenseNumber = dto.LicenseNumber;

            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Updated };
        }

        public async Task<ServiceResponse> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverId == id);
            if (driver == null) return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };

            // Only Admins can delete
            if (!await IsAdminAsync())
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Error, Messages = new List<string> { "Forbidden: You do not have permission to delete this driver." } };

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
        }

        /// <summary>
        /// Returns the current authenticated driver's profile based on Identity linkage.
        /// Requires Driver.ApplicationUserId (string) FK to be present.
        /// </summary>
        public async Task<DriverDto?> Profile()
        {
            if (!await IsDriverAsync()) return null;

            var me = await GetCurrentUserAsync();
            if (me == null) return null;

            var driver = await _context.Drivers
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.ApplicationUserId == me.Id);

            if (driver == null) return null;

            return new DriverDto
            {
                DriverId = driver.DriverId,
                Name = driver.Name,
                LicenseNumber = driver.LicenseNumber
            };
        }

        
        public Task GetDriverForTruck(int id)
        {
            throw new NotImplementedException();
        }
    }
}
