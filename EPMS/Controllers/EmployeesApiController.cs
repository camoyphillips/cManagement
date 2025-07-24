using Microsoft.AspNetCore.Mvc;
using EPMS.DTOs;
using EPMS.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Controllers
{
    // This controller handles API requests (returns JSON instead of views)
    [ApiController]
    [Route("api/[controller]")] // Route will be: /api/EmployeesApi
    public class EmployeesApiController : ControllerBase
    {
        // Service for handling employee operations
        private readonly IEmployeeService _employeeService;

        // Constructor to inject employee service
        public EmployeesApiController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: /api/EmployeesApi
        // Get all employees as JSON
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }

        // GET: /api/EmployeesApi/5
        // Get a single employee by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // POST: /api/EmployeesApi
        // Create a new employee
        [HttpPost]
        public async Task<ActionResult<EmployeeCreateEditDTO>> Create(EmployeeCreateEditDTO dto)
        {
            await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.EmployeeId }, dto);
        }

        // PUT: /api/EmployeesApi/5
        // Update an existing employee
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmployeeCreateEditDTO dto)
        {
            if (id != dto.EmployeeId)
                return BadRequest();

            var updated = await _employeeService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: /api/EmployeesApi/5
        // Delete an employee by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
