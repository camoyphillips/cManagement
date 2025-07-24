using Microsoft.AspNetCore.Mvc;
using EPMS.DTOs;
using EPMS.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Controllers
{
    // This makes the controller an API 
    [ApiController]
    // Sets the base route to: /api/ProjectsApi
    [Route("api/[controller]")]
    public class ProjectsApiController : ControllerBase
    {
        // This service handles all project-related logic
        private readonly IProjectService _projectService;

        // Constructor - gets the project service from the system
        public ProjectsApiController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/ProjectsApi
        // Returns a list of all projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects); // HTTP 200 OK with the list
        }

        // GET: api/ProjectsApi/5
        // Returns one project by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return NotFound(); // HTTP 404 if not found

            return Ok(project); // HTTP 200 OK with the project
        }

        // POST: api/ProjectsApi
        // Adds a new project
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> Create(ProjectDTO dto)
        {
            await _projectService.CreateAsync(dto);
            // Returns HTTP 201 Created with location of new project
            return CreatedAtAction(nameof(GetById), new { id = dto.ProjectId }, dto);
        }

        // PUT: api/ProjectsApi/5
        // Updates an existing project
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProjectDTO dto)
        {
            if (id != dto.ProjectId)
                return BadRequest(); // HTTP 400 if IDs don’t match

            var updated = await _projectService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(); // HTTP 404 if project not found

            return NoContent(); // HTTP 204 success with no content
        }

        // DELETE: api/ProjectsApi/5
        // Deletes a project by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _projectService.DeleteAsync(id);
            if (!deleted)
                return NotFound(); // HTTP 404 if project not found

            return NoContent(); // HTTP 204 after successful delete
        }
    }
}
