using EPMS.DTOs;
using EPMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPMS.Controllers
{
    // This controller handles department pages (list, create, edit, delete)
    public class DepartmentsController : Controller
    {
        // Service used to work with department data
        private readonly IDepartmentService departmentServiceController;

        // Constructor to inject the service
        public DepartmentsController(IDepartmentService departmentService)
        {
            departmentServiceController = departmentService;
        }

        // GET: /Departments
        // Show list of all departments
        public async Task<IActionResult> Index()
        {
            var departments = await departmentServiceController.GetAllAsync();
            return View(departments);
        }

        // GET: /Departments/Create
        // Show form to create a new department
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Departments/Create
        // Save new department after form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentDTO departmentDto)
        {
            if (!ModelState.IsValid)
                return View(departmentDto);

            await departmentServiceController.CreateAsync(departmentDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Departments/Edit/5
        // Show form to edit department with ID = 5
        public async Task<IActionResult> Edit(int id)
        {
            var department = await departmentServiceController.GetByIdAsync(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        // POST: /Departments/Edit/5
        // Update department with ID = 5 after editing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentDTO departmentDto)
        {
            if (id != departmentDto.DepartmentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(departmentDto);

            var updated = await departmentServiceController.UpdateAsync(id, departmentDto);
            if (!updated)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Departments/Delete/5
        // Show confirmation before deleting department with ID = 5
        public async Task<IActionResult> Delete(int id)
        {
            var department = await departmentServiceController.GetByIdAsync(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        // POST: /Departments/Delete/5
        // Delete department with ID = 5 after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await departmentServiceController.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
