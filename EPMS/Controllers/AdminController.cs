using Microsoft.AspNetCore.Mvc;

namespace EPMS.Controllers
{
    // Controller for admin-related pages
    public class AdminController : Controller
    {
        // Show Admin Dashboard
        public IActionResult AdminDashboard()
        {
            return View();
        }

        // Go to Employee list page
        public IActionResult ManageEmployees()
        {
            return RedirectToAction("Index", "Employees");
        }

        // Go to Department list page
        public IActionResult ManageDepartments()
        {
            return RedirectToAction("Index", "Department");
        }

        // Go to Project list page
        public IActionResult ManageProjects()
        {
            return RedirectToAction("Index", "Project");
        }

        // Go to Project Assignment list page
        public IActionResult ViewProjectAssignments()
        {
            return RedirectToAction("Index", "ProjectAssignment");
        }
    }
}
