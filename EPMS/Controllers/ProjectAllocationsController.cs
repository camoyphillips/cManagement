using EPMS.DTOs;
using EPMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace EPMS.Controllers
{
    // This is a  MVC controller that shows views
    public class ProjectAllocationsController : Controller
    {
        // Services to handle business logic for allocations, employees, and projects
        private readonly IEmployeeProjectService employeeProjectServiceController;
        private readonly IEmployeeService employeeServiceController;
        private readonly IProjectService projectServiceController;

        // Constructor - gets all required services using dependency injection
        public ProjectAllocationsController(
            IEmployeeProjectService employeeProjectService,
            IEmployeeService employeeService,
            IProjectService projectService)
        {
            employeeProjectServiceController = employeeProjectService;
            employeeServiceController = employeeService;
            projectServiceController = projectService;
        }

        // GET: /ProjectAllocations
        // Shows the list of all project allocations
        public async Task<IActionResult> Index()
        {
            var allocations = await employeeProjectServiceController.GetAllAllocationsAsync();
            return View(allocations);
        }

        // GET: /ProjectAllocations/Create
        // Shows the form to add a new allocation
        public async Task<IActionResult> Create()
        {
            // Load employee and project lists for dropdowns
            var employees = await employeeServiceController.GetAllAsync();
            var projects = await projectServiceController.GetAllAsync();

            // Store lists in ViewBag so the form can access them
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "FullName");
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName");

            return View(new EmployeeProjectCreateDTO());
        }

        // POST: /ProjectAllocations/Create
        // Handles form submission for new allocation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeProjectCreateDTO allocationDto)
        {
            // If input is not valid, reload dropdowns and return form with errors
            if (!ModelState.IsValid)
            {
                var employees = await employeeServiceController.GetAllAsync();
                var projects = await projectServiceController.GetAllAsync();

                ViewBag.Employees = new SelectList(employees, "EmployeeId", "FullName", allocationDto.EmployeeId);
                ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", allocationDto.ProjectId);

                return View(allocationDto);
            }

            // Add the new allocation
            await employeeProjectServiceController.AddAllocationAsync(allocationDto.EmployeeId, allocationDto.ProjectId);

            // Redirect to the list page
            return RedirectToAction(nameof(Index));
        }

        // GET: /ProjectAllocations/Edit/2/5
        // Shows the form to edit an existing allocation
        public async Task<IActionResult> Edit(int employeeId, int projectId)
        {
            var allocation = await employeeProjectServiceController.GetAllocationAsync(employeeId, projectId);
            if (allocation == null)
                return NotFound();

            var employees = await employeeServiceController.GetAllAsync();
            var projects = await projectServiceController.GetAllAsync();

            ViewBag.Employees = new SelectList(employees, "EmployeeId", "FullName", allocation.EmployeeId);
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", allocation.ProjectId);
            ViewBag.OldEmployeeId = allocation.EmployeeId;
            ViewBag.OldProjectId = allocation.ProjectId;

            var dto = new EmployeeProjectCreateDTO
            {
                EmployeeId = allocation.EmployeeId,
                ProjectId = allocation.ProjectId
            };

            return View(dto);
        }

        // POST: /ProjectAllocations/Edit/2/5
        // Handles form submission to update an allocation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int oldEmployeeId, int oldProjectId, EmployeeProjectCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var employees = await employeeServiceController.GetAllAsync();
                var projects = await projectServiceController.GetAllAsync();

                ViewBag.Employees = new SelectList(employees, "EmployeeId", "FullName", dto.EmployeeId);
                ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", dto.ProjectId);
                ViewBag.OldEmployeeId = oldEmployeeId;
                ViewBag.OldProjectId = oldProjectId;

                return View(dto);
            }

            await employeeProjectServiceController.UpdateAllocationAsync(oldEmployeeId, oldProjectId, dto.EmployeeId, dto.ProjectId);
            return RedirectToAction(nameof(Index));
        }

        // GET: /ProjectAllocations/Delete/2/5
        // Shows a confirmation page before deleting an allocation
        public async Task<IActionResult> Delete(int employeeId, int projectId)
        {
            var allocation = await employeeProjectServiceController.GetAllocationAsync(employeeId, projectId);
            if (allocation == null)
                return NotFound();

            return View(allocation);
        }

        // POST: /ProjectAllocations/Delete/2/5
        // Deletes the allocation after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeId, int projectId)
        {
            await employeeProjectServiceController.RemoveAllocationAsync(employeeId, projectId);
            return RedirectToAction(nameof(Index));
        }
    }
}
