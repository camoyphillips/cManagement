using EPMS.DTOs;
using EPMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Controllers
{
    // This makes the controller return JSON instead of views
    [ApiController]
    // This sets the base route: /api/ProjectAllocationsApi
    [Route("api/[controller]")]
    public class ProjectAllocationsApiController : ControllerBase
    {
        // Service used to handle project allocations
        private readonly IEmployeeProjectService _employeeProjectService;

        // Constructor to get the allocation service
        public ProjectAllocationsApiController(IEmployeeProjectService employeeProjectService)
        {
            _employeeProjectService = employeeProjectService;
        }

        // GET: /api/ProjectAllocationsApi
        // Get all employee-project allocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeProjectDTO>>> GetAll()
        {
            var allocations = await _employeeProjectService.GetAllAllocationsAsync();
            return Ok(allocations);
        }

        // GET: /api/ProjectAllocationsApi/2/5
        // Get one allocation using employeeId and projectId
        [HttpGet("{employeeId:int}/{projectId:int}")]
        public async Task<ActionResult<EmployeeProjectDTO>> Get(int employeeId, int projectId)
        {
            var allocation = await _employeeProjectService.GetAllocationAsync(employeeId, projectId);
            if (allocation == null)
                return NotFound();

            return Ok(allocation);
        }

        // POST: /api/ProjectAllocationsApi
        // Create a new allocation (assign employee to project)
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeProjectCreateDTO dto)
        {
            await _employeeProjectService.AddAllocationAsync(dto.EmployeeId, dto.ProjectId);
            return CreatedAtAction(nameof(Get), new { employeeId = dto.EmployeeId, projectId = dto.ProjectId }, dto);
        }

        // PUT: /api/ProjectAllocationsApi/2/5
        // Update an existing allocation (change employee/project)
        [HttpPut("{oldEmployeeId:int}/{oldProjectId:int}")]
        public async Task<IActionResult> Update(int oldEmployeeId, int oldProjectId, EmployeeProjectCreateDTO dto)
        {
            await _employeeProjectService.UpdateAllocationAsync(oldEmployeeId, oldProjectId, dto.EmployeeId, dto.ProjectId);
            return NoContent();
        }

        // DELETE: /api/ProjectAllocationsApi/2/5
        // Delete an allocation using employeeId and projectId
        [HttpDelete("{employeeId:int}/{projectId:int}")]
        public async Task<IActionResult> Delete(int employeeId, int projectId)
        {
            await _employeeProjectService.RemoveAllocationAsync(employeeId, projectId);
            return NoContent();
        }
    }
}
