using Microsoft.AspNetCore.Mvc;
using EPMS.DTOs;
using EPMS.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Controllers
{
    [ApiController]

    // This sets the route for this controller: /api/DepartmentsApi
    [Route("api/[controller]")]
    public class DepartmentsApiController : ControllerBase
    {
        // This is a reference to a service that handles department data (like get, add, update, delete)
        private readonly IDepartmentService _departmentService;

        // Constructor to inject the department service
        public DepartmentsApiController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/DepartmentsApi
        // This method gets all the departments from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAll()
        {
            var departments = await _departmentService.GetAllAsync(); 
            return Ok(departments);
        }

        // GET: api/DepartmentsApi/5
        // This method gets one department by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id); // Find department by ID
            if (department == null)
                return NotFound(); 

            return Ok(department); 
        }

        // POST: api/DepartmentsApi
        // This method adds a new department to the database
        [HttpPost]
        public async Task<ActionResult> Create(DepartmentDTO dto)
        {
            await _departmentService.CreateAsync(dto); // Save new department
            return CreatedAtAction(nameof(GetById), new { id = dto.DepartmentId }, dto);
        }

        // PUT: api/DepartmentsApi/5
        // This method updates an existing department
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, DepartmentDTO dto)
        {
            // If ID in URL doesn't match the ID in the data, return bad request
            if (id != dto.DepartmentId)
                return BadRequest();

            var updated = await _departmentService.UpdateAsync(id, dto); 
            if (!updated)
                return NotFound(); 

            return NoContent();
        }

        // DELETE: api/DepartmentsApi/5
        // This method deletes a department by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _departmentService.DeleteAsync(id); 
            if (!deleted)
                return NotFound(); 

            return NoContent(); 
        }
    }
}
