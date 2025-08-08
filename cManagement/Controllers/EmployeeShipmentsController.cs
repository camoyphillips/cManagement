using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cManagement.Data;
using cManagement.Models;
using cManagement.Models.Dtos; 
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

        // DTO for returning detailed employee-shipment assignments.
        // This is a direct parallel to the provided DriverShipmentDto.
        public class EmployeeShipmentDto
        {
            public int? EmployeeShipmentId { get; set; }
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

        /// <summary>
        /// Retrieves a list of all employee-shipment assignments.
        /// </summary>
        /// <returns>A list of EmployeeShipmentDto objects.</returns>
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
                        EmployeeShipmentId = es.EmployeeShipmentId,
                        EmployeeId = es.EmployeeId,
                        EmployeeFullName = es.Employee.FullName,
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

        /// <summary>
        /// Retrieves a specific employee-shipment assignment by ID.
        /// </summary>
        /// <param name="id">The ID of the assignment.</param>
        /// <returns>The EmployeeShipmentDto for the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeShipmentDto>> GetEmployeeShipment(int id)
        {
            try
            {
                var employeeShipment = await _context.EmployeeShipments
                    .Include(es => es.Employee)
                    .Include(es => es.Shipment)
                    .FirstOrDefaultAsync(es => es.EmployeeShipmentId == id);

                if (employeeShipment == null)
                    return NotFound($"Employee-Shipment assignment with ID {id} not found.");

                var dto = new EmployeeShipmentDto
                {
                    EmployeeShipmentId = employeeShipment.EmployeeShipmentId,
                    EmployeeId = employeeShipment.EmployeeId,
                    EmployeeFullName = employeeShipment.Employee.FullName,
                    ShipmentId = employeeShipment.ShipmentId,
                    ShipmentOrigin = employeeShipment.Shipment.Origin,
                    ShipmentDestination = employeeShipment.Shipment.Destination,
                    Role = employeeShipment.Role,
                    Status = employeeShipment.Status,
                    AssignedOn = employeeShipment.AssignedOn,
                    CompletedAt = employeeShipment.CompletedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeeShipment: {ex.Message}");
                return StatusCode(500, $"Error retrieving assignment with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new employee-shipment assignment.
        /// </summary>
        /// <param name="employeeShipmentDto">The DTO containing the assignment data.</param>
        /// <returns>The newly created EmployeeShipmentDto.</returns>
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeShipmentDto>> PostEmployeeShipment([FromBody] EmployeeShipmentDto employeeShipmentDto)
        {
            employeeShipmentDto.EmployeeShipmentId = null;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.EmployeeShipments.AnyAsync(es => es.EmployeeId == employeeShipmentDto.EmployeeId && es.ShipmentId == employeeShipmentDto.ShipmentId))
            {
                ModelState.AddModelError("", "This employee is already assigned to this shipment.");
                return BadRequest(ModelState);
            }

            try
            {
                var employeeShipment = new EmployeeShipment
                {
                    EmployeeId = employeeShipmentDto.EmployeeId,
                    ShipmentId = employeeShipmentDto.ShipmentId,
                    Role = employeeShipmentDto.Role,
                    Status = employeeShipmentDto.Status,
                    CompletedAt = employeeShipmentDto.CompletedAt,
                    AssignedOn = employeeShipmentDto.AssignedOn == default ? DateTime.UtcNow : employeeShipmentDto.AssignedOn
                };

                _context.EmployeeShipments.Add(employeeShipment);
                await _context.SaveChangesAsync();

                var createdDto = await GetEmployeeShipment(employeeShipment.EmployeeShipmentId);
                if (createdDto.Result is OkObjectResult okResult && okResult.Value is EmployeeShipmentDto populatedDto)
                {
                    return CreatedAtAction(nameof(GetEmployeeShipment), new { id = populatedDto.EmployeeShipmentId }, populatedDto);
                }

                employeeShipmentDto.EmployeeShipmentId = employeeShipment.EmployeeShipmentId;
                return CreatedAtAction(nameof(GetEmployeeShipment), new { id = employeeShipment.EmployeeShipmentId }, employeeShipmentDto);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in PostEmployeeShipment: {ex.Message}");
                if (ex.InnerException?.Message?.Contains("FOREIGN KEY") == true)
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

        /// <summary>
        /// Updates an existing employee-shipment assignment.
        /// </summary>
        /// <param name="id">The ID of the assignment to update.</param>
        /// <param name="employeeShipmentDto">The DTO with updated assignment data.</param>
        /// <returns>NoContent if successful, or an appropriate error response.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeShipment(int id, [FromBody] EmployeeShipmentDto employeeShipmentDto)
        {
            if (id != employeeShipmentDto.EmployeeShipmentId)
                return BadRequest("ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeeShipment = await _context.EmployeeShipments.FindAsync(id);
            if (employeeShipment == null)
                return NotFound($"Assignment with ID {id} not found.");

            if ((employeeShipment.EmployeeId != employeeShipmentDto.EmployeeId || employeeShipment.ShipmentId != employeeShipmentDto.ShipmentId)
                && await _context.EmployeeShipments.AnyAsync(es => es.EmployeeId == employeeShipmentDto.EmployeeId && es.ShipmentId == employeeShipmentDto.ShipmentId && es.EmployeeShipmentId != id))
            {
                ModelState.AddModelError("", "Assignment already exists with this EmployeeId and ShipmentId.");
                return BadRequest(ModelState);
            }

            if (!await _context.Employees.AnyAsync(e => e.EmployeeId == employeeShipmentDto.EmployeeId))
            {
                ModelState.AddModelError(nameof(employeeShipmentDto.EmployeeId), "Employee does not exist.");
                return BadRequest(ModelState);
            }

            if (!await _context.Shipments.AnyAsync(s => s.ShipmentId == employeeShipmentDto.ShipmentId))
            {
                ModelState.AddModelError(nameof(employeeShipmentDto.ShipmentId), "Shipment does not exist.");
                return BadRequest(ModelState);
            }

            employeeShipment.EmployeeId = employeeShipmentDto.EmployeeId;
            employeeShipment.ShipmentId = employeeShipmentDto.ShipmentId;
            employeeShipment.Role = employeeShipmentDto.Role;
            employeeShipment.Status = employeeShipmentDto.Status;
            employeeShipment.CompletedAt = employeeShipmentDto.CompletedAt;
            employeeShipment.AssignedOn = employeeShipmentDto.AssignedOn;

            _context.Entry(employeeShipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeShipmentExists(id))
                    return NotFound();
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in PutEmployeeShipment: {ex.Message}");
                return StatusCode(500, $"Update failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an employee-shipment assignment by ID.
        /// </summary>
        /// <param name="id">The ID of the assignment to delete.</param>
        /// <returns>NoContent if successful, or an appropriate error response.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeShipment(int id)
        {
            try
            {
                var employeeShipment = await _context.EmployeeShipments.FindAsync(id);
                if (employeeShipment == null)
                    return NotFound($"Assignment with ID {id} not found.");

                _context.EmployeeShipments.Remove(employeeShipment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in DeleteEmployeeShipment: {ex.Message}");
                return StatusCode(500, $"Delete failed: {ex.Message}");
            }
        }

        private bool EmployeeShipmentExists(int id)
        {
            return _context.EmployeeShipments.Any(e => e.EmployeeShipmentId == id);
        }
    }
}