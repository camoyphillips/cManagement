using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cManagement.Data;
using cManagement.Models;
using cManagement.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace cManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverShipmentsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<DriverShipmentDto>>> GetDriverShipments()
        {
            try
            {
                var driverShipments = await _context.DriverShipments
                    .Include(ds => ds.Driver)
                    .Include(ds => ds.Shipment)
                    .Select(ds => new DriverShipmentDto
                    {
                        DriverShipmentId = ds.DriverShipmentId,
                        DriverId = ds.DriverId,
                        DriverName = ds.Driver.Name,
                        ShipmentId = ds.ShipmentId,
                        ShipmentOrigin = ds.Shipment.Origin,
                        ShipmentDestination = ds.Shipment.Destination,
                        Role = ds.Role,
                        AssignedOn = ds.AssignedOn
                    })
                    .ToListAsync();

                return Ok(driverShipments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDriverShipments: {ex.Message}");
                return StatusCode(500, $"An error occurred while retrieving driver shipments: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverShipmentDto>> GetDriverShipment(int id)
        {
            try
            {
                var driverShipment = await _context.DriverShipments
                    .Include(ds => ds.Driver)
                    .Include(ds => ds.Shipment)
                    .FirstOrDefaultAsync(ds => ds.DriverShipmentId == id);

                if (driverShipment == null)
                    return NotFound($"Driver-Shipment assignment with ID {id} not found.");

                var dto = new DriverShipmentDto
                {
                    DriverShipmentId = driverShipment.DriverShipmentId,
                    DriverId = driverShipment.DriverId,
                    DriverName = driverShipment.Driver.Name,
                    ShipmentId = driverShipment.ShipmentId,
                    ShipmentOrigin = driverShipment.Shipment.Origin,
                    ShipmentDestination = driverShipment.Shipment.Destination,
                    Role = driverShipment.Role,
                    AssignedOn = driverShipment.AssignedOn
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDriverShipment: {ex.Message}");
                return StatusCode(500, $"Error retrieving assignment with ID {id}: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<DriverShipmentDto>> PostDriverShipment([FromBody] DriverShipmentDto driverShipmentDto)
        {
            driverShipmentDto.DriverShipmentId = null;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.DriverShipments.AnyAsync(ds => ds.DriverId == driverShipmentDto.DriverId && ds.ShipmentId == driverShipmentDto.ShipmentId))
            {
                ModelState.AddModelError("", "This driver is already assigned to this shipment.");
                return BadRequest(ModelState);
            }

            try
            {
                var driverShipment = new DriverShipment
                {
                    DriverId = driverShipmentDto.DriverId,
                    ShipmentId = driverShipmentDto.ShipmentId,
                    Role = driverShipmentDto.Role,
                    AssignedOn = driverShipmentDto.AssignedOn == default ? DateTime.UtcNow : driverShipmentDto.AssignedOn
                };

                _context.DriverShipments.Add(driverShipment);
                await _context.SaveChangesAsync();

                var createdDto = await GetDriverShipment(driverShipment.DriverShipmentId);
                if (createdDto.Result is OkObjectResult okResult && okResult.Value is DriverShipmentDto populatedDto)
                {
                    return CreatedAtAction(nameof(GetDriverShipment), new { id = populatedDto.DriverShipmentId }, populatedDto);
                }

                driverShipmentDto.DriverShipmentId = driverShipment.DriverShipmentId;
                return CreatedAtAction(nameof(GetDriverShipment), new { id = driverShipment.DriverShipmentId }, driverShipmentDto);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in PostDriverShipment: {ex.Message}");
                if (ex.InnerException?.Message?.Contains("FOREIGN KEY") == true)
                {
                    ModelState.AddModelError("", "Invalid DriverId or ShipmentId.");
                    return BadRequest(ModelState);
                }
                return StatusCode(500, $"Error creating assignment: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostDriverShipment: {ex.Message}");
                return StatusCode(500, $"Error creating assignment: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverShipment(int id, [FromBody] DriverShipmentDto driverShipmentDto)
        {
            if (id != driverShipmentDto.DriverShipmentId)
                return BadRequest("ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driverShipment = await _context.DriverShipments.FindAsync(id);
            if (driverShipment == null)
                return NotFound($"Assignment with ID {id} not found.");

            if ((driverShipment.DriverId != driverShipmentDto.DriverId || driverShipment.ShipmentId != driverShipmentDto.ShipmentId)
                && await _context.DriverShipments.AnyAsync(ds => ds.DriverId == driverShipmentDto.DriverId && ds.ShipmentId == driverShipmentDto.ShipmentId && ds.DriverShipmentId != id))
            {
                ModelState.AddModelError("", "Assignment already exists with this DriverId and ShipmentId.");
                return BadRequest(ModelState);
            }

            if (!await _context.Drivers.AnyAsync(d => d.DriverId == driverShipmentDto.DriverId))
            {
                ModelState.AddModelError(nameof(driverShipmentDto.DriverId), "Driver does not exist.");
                return BadRequest(ModelState);
            }

            if (!await _context.Shipments.AnyAsync(s => s.ShipmentId == driverShipmentDto.ShipmentId))
            {
                ModelState.AddModelError(nameof(driverShipmentDto.ShipmentId), "Shipment does not exist.");
                return BadRequest(ModelState);
            }

            driverShipment.DriverId = driverShipmentDto.DriverId;
            driverShipment.ShipmentId = driverShipmentDto.ShipmentId;
            driverShipment.Role = driverShipmentDto.Role;
            driverShipment.AssignedOn = driverShipmentDto.AssignedOn;

            _context.Entry(driverShipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverShipmentExists(id))
                    return NotFound();

                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in PutDriverShipment: {ex.Message}");
                return StatusCode(500, $"Update failed: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverShipment(int id)
        {
            try
            {
                var driverShipment = await _context.DriverShipments.FindAsync(id);
                if (driverShipment == null)
                    return NotFound($"Assignment with ID {id} not found.");

                _context.DriverShipments.Remove(driverShipment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in DeleteDriverShipment: {ex.Message}");
                return StatusCode(500, $"Delete failed: {ex.Message}");
            }
        }

        private bool DriverShipmentExists(int id)
        {
            return _context.DriverShipments.Any(e => e.DriverShipmentId == id);
        }
    }
}
