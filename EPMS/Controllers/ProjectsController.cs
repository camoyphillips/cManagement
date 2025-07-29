using EPMS.DTOs;
using EPMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPMS.Controllers
{
    public class ProjectsController : Controller
    {
        // This service handles all project-related operations
        private readonly IProjectService projectServiceController;

        // Constructor - gets the project service from the system
        public ProjectsController(IProjectService projectService)
        {
            projectServiceController = projectService;
        }

        // GET: /Projects
        // Shows a list of all projects
        public async Task<IActionResult> Index()
        {
            var projects = await projectServiceController.GetAllAsync();
            return View(projects); 
        }

        // GET: /Projects/Create
        // Shows a form to create a new project
        public IActionResult Create()
        {
            return View(); 
        }

        // POST: /Projects/Create
        // Saves the new project after form is submitted
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectDTO projectDto)
        {

            if (!ModelState.IsValid)
                return View(projectDto);
            await projectServiceController.CreateAsync(projectDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Projects/Edit/5
        // Shows a form to edit a project by its ID
        public async Task<IActionResult> Edit(int id)
        {
            var project = await projectServiceController.GetByIdAsync(id);
            if (project == null)
                return NotFound();

            return View(project); 
        }

        // POST: /Projects/Edit/5
        // Saves the changes to the project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectDTO projectDto)
        {
            if (id != projectDto.ProjectId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(projectDto);

            // Update the project
            var updated = await projectServiceController.UpdateAsync(id, projectDto);
            if (!updated)
                return NotFound(); 

            return RedirectToAction(nameof(Index));
        }

        // GET: /Projects/Delete/5
        // Shows a confirmation page before deleting
        public async Task<IActionResult> Delete(int id)
        {
            var project = await projectServiceController.GetByIdAsync(id);
            if (project == null)
                return NotFound(); 

            return View(project); 
        }

        // POST: /Projects/Delete/5
        // Deletes the project after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await projectServiceController.DeleteAsync(id);
            if (!deleted)
                return NotFound(); 

            return RedirectToAction(nameof(Index));
        }
    }
}
