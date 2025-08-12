using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cManagement.Data;
using cManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskStatus = cManagement.Models.TaskStatus;

namespace cManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeShipmentsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // DTO for returning detailed employee-shipment assignments (no single PK).
        public class EmployeeShipmentDto
        {
            public int EmployeeId { get; set; }
            public string EmployeeFullName { get; set; } = string.Empty;
            public int ShipmentId { get; set; }
            public string ShipmentOrigin { get; set; } = string.Empty;
            public string ShipmentDestination { get; set; } = string.Empty;
            public string? Role { get; set; }
            public TaskStatus Status { get; set; }
            public DateTime AssignedOn { get; set; }
            public DateTime? CompletedAt { get; set; }
        }

        /// <summary>Retrieves a list of all employee-shipment assignments.</summary>
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<EmployeeShipmentDto>>> GetEmployeeShipments()
        {
            try
            {
                var employeeShipments = await _context.EmployeeShipments
                    .Include(es => es.Employee)
                    .Include(es => es.Shipment)
                    .Select(es => new EmployeeShipmentDto
                    {
                        EmployeeId = es.EmployeeId,
                        EmployeeFullName = (es.Employee.FirstName + " " + es.Employee.LastName),
                        ShipmentId = es.ShipmentId,
                        ShipmentOrigin = es.Shipment.Origin,
                        ShipmentDestination = es.Shipment.Destination,
                        Role = es.Role,
                        Status = es.Status,
                        AssignedOn = es.AssignedOn,
                        CompletedAt = es.CompletedAt
                    })
                    .ToListAsync();

                return Ok(employeeShipments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeeShipments: {ex.Message}");
                return StatusCode(500, $"An error occurred while retrieving employee shipments: {ex.Message}");
            }
        }

        /// <summary>Retrieves a specific employee-shipment assignment by composite key.</summary>
        [HttpGet("{employeeId:int}/{shipmentId:int}")]
        public async Task<ActionResult<EmployeeShipmentDto>> GetEmployeeShipment(int employeeId, int shipmentId)
        {
            try
            {
                var es = await _context.EmployeeShipments
                    .Include(x => x.Employee)
                    .Include(x => x.Shipment)
                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.ShipmentId == shipmentId);

                if (es == null)
                    return NotFound($"Employee-Shipment assignment {employeeId}/{shipmentId} not found.");

                var dto = new EmployeeShipmentDto
                {
                    EmployeeId = es.EmployeeId,
                    EmployeeFullName = (es.Employee.FirstName + " " + es.Employee.LastName),
                    ShipmentId = es.ShipmentId,
                    ShipmentOrigin = es.Shipment.Origin,
                    ShipmentDestination = es.Shipment.Destination,
                    Role = es.Role,
                    Status = es.Status,
                    AssignedOn = es.AssignedOn,
                    CompletedAt = es.CompletedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeeShipment: {ex.Message}");
                return StatusCode(500, $"Error retrieving assignment {employeeId}/{shipmentId}: {ex.Message}");
            }
        }

        /// <summary>Creates a new employee-shipment assignment.</summary>
        [HttpPost("create")]
        public async Task<ActionResult<EmployeeShipmentDto>> PostEmployeeShipment([FromBody] EmployeeShipmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate FKs and uniqueness for composite key
            if (!await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId))
            {
                ModelState.AddModelError(nameof(dto.EmployeeId), "Employee does not exist.");
                return BadRequest(ModelState);
            }
            if (!await _context.Shipments.AnyAsync(s => s.ShipmentId == dto.ShipmentId))
            {
                ModelState.AddModelError(nameof(dto.ShipmentId), "Shipment does not exist.");
                return BadRequest(ModelState);
            }
            if (await _context.EmployeeShipments.AnyAsync(es => es.EmployeeId == dto.EmployeeId && es.ShipmentId == dto.ShipmentId))
            {
                ModelState.AddModelError("", "This employee is already assigned to this shipment.");
                return BadRequest(ModelState);
            }

            try
            {
                var entity = new EmployeeShipment
                {
                    EmployeeId = dto.EmployeeId,
                    ShipmentId = dto.ShipmentId,
                    Role = dto.Role,
                    Status = dto.Status,
                    CompletedAt = dto.CompletedAt,
                    AssignedOn = dto.AssignedOn == default ? DateTime.UtcNow : dto.AssignedOn
                };

                _context.EmployeeShipments.Add(entity);
                await _context.SaveChangesAsync();

                // Return the fully populated DTO
                return CreatedAtAction(
                    nameof(GetEmployeeShipment),
                    new { employeeId = entity.EmployeeId, shipmentId = entity.ShipmentId },
                    await BuildDtoAsync(entity.EmployeeId, entity.ShipmentId));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in PostEmployeeShipment: {ex.Message}");
                if (ex.InnerException?.Message?.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError("", "Invalid EmployeeId or ShipmentId.");
                    return BadRequest(ModelState);
                }
                return StatusCode(500, $"Error creating assignment: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostEmployeeShipment: {ex.Message}");
                return StatusCode(500, $"Error creating assignment: {ex.Message}");
            }
        }

        /// <summary>Updates an existing employee-shipment assignment (keys cannot change).</summary>
        [HttpPut("{employeeId:int}/{shipmentId:int}")]
        public async Task<IActionResult> PutEmployeeShipment(int employeeId, int shipmentId, [FromBody] EmployeeShipmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Enforce keys match route; with composite PK we don't allow changing keys in-place
            if (dto.EmployeeId != employeeId || dto.ShipmentId != shipmentId)
                return BadRequest("Composite keys cannot be changed. Delete and recreate the assignment instead.");

            var entity = await _context.EmployeeShipments
                .FirstOrDefaultAsync(es => es.EmployeeId == employeeId && es.ShipmentId == shipmentId);

            if (entity == null)
                return NotFound($"Assignment {employeeId}/{shipmentId} not found.");

            // Validate FKs (existence)
            if (!await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId))
            {
                ModelState.AddModelError(nameof(dto.EmployeeId), "Employee does not exist.");
                return BadRequest(ModelState);
            }
            if (!await _context.Shipments.AnyAsync(s => s.ShipmentId == dto.ShipmentId))
            {
                ModelState.AddModelError(nameof(dto.ShipmentId), "Shipment does not exist.");
                return BadRequest(ModelState);
            }

            entity.Role = dto.Role;
            entity.Status = dto.Status;
            entity.CompletedAt = dto.CompletedAt;
            entity.AssignedOn = dto.AssignedOn;

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeShipmentExists(employeeId, shipmentId))
                    return NotFound();
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in PutEmployeeShipment: {ex.Message}");
                return StatusCode(500, $"Update failed: {ex.Message}");
            }
        }

        /// <summary>Deletes an employee-shipment assignment by composite key.</summary>
        [HttpDelete("{employeeId:int}/{shipmentId:int}")]
        public async Task<IActionResult> DeleteEmployeeShipment(int employeeId, int shipmentId)
        {
            try
            {
                var entity = await _context.EmployeeShipments
                    .FirstOrDefaultAsync(es => es.EmployeeId == employeeId && es.ShipmentId == shipmentId);

                if (entity == null)
                    return NotFound($"Assignment {employeeId}/{shipmentId} not found.");

                _context.EmployeeShipments.Remove(entity);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in DeleteEmployeeShipment: {ex.Message}");
                return StatusCode(500, $"Delete failed: {ex.Message}");
            }
        }

        // Helpers
        private bool EmployeeShipmentExists(int employeeId, int shipmentId) =>
            _context.EmployeeShipments.Any(e => e.EmployeeId == employeeId && e.ShipmentId == shipmentId);

        private async Task<EmployeeShipmentDto> BuildDtoAsync(int employeeId, int shipmentId)
        {
            var es = await _context.EmployeeShipments
                .Include(x => x.Employee)
                .Include(x => x.Shipment)
                .FirstAsync(x => x.EmployeeId == employeeId && x.ShipmentId == shipmentId);

            return new EmployeeShipmentDto
            {
                EmployeeId = es.EmployeeId,
                EmployeeFullName = (es.Employee.FirstName + " " + es.Employee.LastName),
                ShipmentId = es.ShipmentId,
                ShipmentOrigin = es.Shipment.Origin,
                ShipmentDestination = es.Shipment.Destination,
                Role = es.Role,
                Status = es.Status,
                AssignedOn = es.AssignedOn,
                CompletedAt = es.CompletedAt
            };
        }
    }
}
