using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cManagement.Models;
using cManagement.Services.Implementations; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace cManagement.Data
{
    /// <summary>
    /// Database initializer for cManagement.
    /// Call from Program.cs (after building the app):
    /// using (var scope = app.Services.CreateScope())
    /// {
    ///     await DbInitializer.SeedAsync(scope.ServiceProvider, app.Configuration);
    /// }
    /// </summary>
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration? config = null)
        {
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");

            // 1) Migrate both contexts 
            var appCtx = services.GetRequiredService<ApplicationDbContext>();
            var domainCtx = services.GetRequiredService<cManagementDbContext>();

            await appCtx.Database.MigrateAsync();
            await domainCtx.Database.MigrateAsync();

            // 2) Roles
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "Manager", "Dispatcher", "Driver" };
            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(r));
                    if (!result.Succeeded)
                        logger.LogWarning("Failed creating role {Role}: {Errors}", r, string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }

            // 3) Admin user
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var adminEmail = config?["Seed:AdminEmail"] ?? "admin@cmanagement.local";
            var adminPwd = config?["Seed:AdminPassword"] ?? "Change_this_password_123!";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var create = await userManager.CreateAsync(admin, adminPwd);
                if (!create.Succeeded)
                {
                    logger.LogError("Failed creating admin user: {Errors}", string.Join("; ", create.Errors.Select(e => e.Description)));
                    // Bail out early if admin creation failed—domain seeds rely on ApplicationUserId
                    return;
                }
            }

            // Ensure Admin role
            if (!await userManager.IsInRoleAsync(admin, "Admin"))
                await userManager.AddToRoleAsync(admin, "Admin");

            // 4) Seed Drivers (link to admin.ApplicationUserId). Idempotent on LicenseNumber.
            var sampleDrivers = new List<Driver>
            {
                new() { Name = "Christine", LicenseNumber = "ABC",     ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Sean",      LicenseNumber = "DEF",     ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Camoy",     LicenseNumber = "GHI789",  ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Khani",     LicenseNumber = "JKL012",  ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Phillips",  LicenseNumber = "MNO345",  ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Gary",      LicenseNumber = "PQR678",  ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Lance",     LicenseNumber = "STU901",  ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Doreen",    LicenseNumber = "YZA567",  ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Sophia",    LicenseNumber = "FGH654",  ContactNumber = null, ApplicationUserId = admin.Id },
                new() { Name = "Logan",     LicenseNumber = "MNO987",  ContactNumber = null, ApplicationUserId = admin.Id },
            };

            foreach (var d in sampleDrivers)
            {
                var exists = await domainCtx.Drivers.AnyAsync(x => x.LicenseNumber == d.LicenseNumber);
                if (!exists)
                    domainCtx.Drivers.Add(d);
            }
            await domainCtx.SaveChangesAsync();

            // 5) Optional Shipments seed (reflection-based to avoid compile-time coupling)
            //    Tries common property names: ReferenceNumber/Ref, Origin, Destination,
            //    PickupDate/PickupAt, DeliveryDate/DeliveryAt, Status, ApplicationUserId.
            try
            {
                if (!await domainCtx.Set<Shipment>().AnyAsync())
                {
                    var shipments = new List<object>();
                    shipments.Add(CreateShipmentDynamic(admin.Id,
                        reference: "SHP-2025-0001",
                        origin: "Toronto, ON",
                        destination: "Montreal, QC",
                        pickup: new DateTime(2025, 8, 10),
                        delivery: new DateTime(2025, 8, 12),
                        status: "Delivered"));

                    shipments.Add(CreateShipmentDynamic(admin.Id,
                        reference: "SHP-2025-0002",
                        origin: "Toronto, ON",
                        destination: "Ottawa, ON",
                        pickup: new DateTime(2025, 8, 11),
                        delivery: new DateTime(2025, 8, 12),
                        status: "Delivered"));

                    shipments.Add(CreateShipmentDynamic(admin.Id,
                        reference: "SHP-2025-0003",
                        origin: "Mississauga, ON",
                        destination: "Quebec City, QC",
                        pickup: new DateTime(2025, 8, 12),
                        delivery: new DateTime(2025, 8, 14),
                        status: "InTransit"));

                    foreach (var s in shipments)
                        domainCtx.Add(s);

                    await domainCtx.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Skip if your Shipment entity has different required fields.
                logger.LogWarning(ex, "Shipment seed skipped: adjust property names/requirements if needed.");
            }

            logger.LogInformation("Seed completed.");
        }

        /// <summary>
        /// Creates a Shipment instance and sets common properties if they exist.
        /// </summary>
        private static Shipment CreateShipmentDynamic(string applicationUserId,
            string reference, string origin, string destination,
            DateTime pickup, DateTime? delivery, string status)
        {
            var s = new Shipment();

            // Helper local function
            static void Set(object target, string prop, object? value)
            {
                var p = target.GetType().GetProperty(prop);
                if (p != null && p.CanWrite)
                {
                    // Handle nullable DateTime conversion where needed
                    if (value is DateTime dt && p.PropertyType == typeof(DateTime?))
                        p.SetValue(target, (DateTime?)dt);
                    else
                        p.SetValue(target, value);
                }
            }

            // Try common names
            Set(s, "ReferenceNumber", reference);
            Set(s, "Reference", reference);
            Set(s, "Ref", reference);

            Set(s, "Origin", origin);
            Set(s, "From", origin);
            Set(s, "FromLocation", origin);
            Set(s, "OriginCity", origin);
            Set(s, "PickupLocation", origin);

            Set(s, "Destination", destination);
            Set(s, "To", destination);
            Set(s, "ToLocation", destination);
            Set(s, "DestinationCity", destination);
            Set(s, "DropoffLocation", destination);

            Set(s, "PickupDate", pickup);
            Set(s, "PickupAt", pickup);
            Set(s, "PickupDateTime", pickup);
            Set(s, "ShipDate", pickup);
            Set(s, "StartDate", pickup);

            if (delivery.HasValue)
            {
                Set(s, "DeliveryDate", delivery.Value);
                Set(s, "DeliveryAt", delivery.Value);
                Set(s, "DeliveryDateTime", delivery.Value);
                Set(s, "DropoffDate", delivery.Value);
                Set(s, "ArrivalDate", delivery.Value);
                Set(s, "EndDate", delivery.Value);
            }

            Set(s, "Status", status);
            Set(s, "ShipmentStatus", status);
            Set(s, "State", status);

            Set(s, "ApplicationUserId", applicationUserId);
            Set(s, "AppUserId", applicationUserId);
            Set(s, "UserId", applicationUserId);

            return s;
        }
    }
}
