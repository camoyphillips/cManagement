using System.ComponentModel.DataAnnotations;

namespace EPMS.DTOs
{
    // This class is for showing and sending employee data to views or APIs
    public class EmployeeDTO
    {
        // This is the ID of the employee
        public int EmployeeId { get; set; }

        // This is the first name of the employee
        // It must be filled in and can’t go over 50 characters
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        // This is the last name of the employee
        // Also required and max 50 characters allowed
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        // This is the email of the employee
        // It must be filled in and should be in correct email format
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        // This shows which department the employee belongs to
        // It’s required and we use this in dropdowns to select a department
        [Required(ErrorMessage = "Please select a department.")]
        public int? DepartmentId { get; set; }

        // This combines first name and last name for display
        public string FullName => FirstName + " " + LastName;

        // This is the name of the department (not just the ID)
        public string DepartmentName { get; set; }

        // This is a list of project names that the employee is working on
        public List<string> ProjectNames { get; set; }
    }
}
