using EPMS.DTOs;
using EPMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Controllers
{
    // This controller handles web pages for managing employees
    public class EmployeesController : Controller
    {
        // Services for employee and department operations
        private readonly IEmployeeService employeeServiceController;
        private readonly IDepartmentService departmentServiceController;

        // Inject services through constructor
        public EmployeesController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            employeeServiceController = employeeService;
            departmentServiceController = departmentService;
        }

        // GET: /Employees
        // Show all employees
        public async Task<IActionResult> Index()
        {
            IEnumerable<EmployeeDTO> employees = await employeeServiceController.GetAllAsync();
            return View(employees);
        }

        // GET: /Employees/Details/5
        // Show details of a specific employee
        public async Task<IActionResult> Details(int id)
        {
            var employee = await employeeServiceController.GetEmployeeDetailsAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // GET: /Employees/Create
        // Show form to add new employee
        public async Task<IActionResult> Create()
        {
            await PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: /Employees/Create
        // Save new employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateEditDTO employeeDto)
        {
            if (ModelState.IsValid)
            {
                await employeeServiceController.CreateAsync(employeeDto);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDepartmentsDropDownList(employeeDto.DepartmentId);
            return View(employeeDto);
        }

        // GET: /Employees/Edit/5
        // Show form to edit employee with ID = 5
        public async Task<IActionResult> Edit(int id)
        {
            var employeeDto = await employeeServiceController.GetByIdForEditAsync(id);
            if (employeeDto == null)
                return NotFound();

            await PopulateDepartmentsDropDownList(employeeDto.DepartmentId);
            return View(employeeDto);
        }

        // POST: /Employees/Edit/5
        // Save changes for employee with ID = 5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeCreateEditDTO employeeDto)
        {
            if (id != employeeDto.EmployeeId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var result = await employeeServiceController.UpdateAsync(id, employeeDto);
                if (!result)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }

            await PopulateDepartmentsDropDownList(employeeDto.DepartmentId);
            return View(employeeDto);
        }

        // GET: /Employees/Delete/5
        // Show confirmation before deleting employee with ID = 5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await employeeServiceController.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: /Employees/Delete/5
        // Delete employee with ID = 5 after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await employeeServiceController.DeleteAsync(id);
            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // Fill department dropdown for create/edit forms
        private async Task PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departments = await departmentServiceController.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", selectedDepartment);
        }
    }
}
