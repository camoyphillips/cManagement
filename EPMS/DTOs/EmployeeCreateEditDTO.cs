namespace EPMS.DTOs
{
    // This class is used when  want to add or edit an employee
    public class EmployeeCreateEditDTO
    {
        // This is the ID of the employee
        // We  need this while editing, not adding a new employee
        public int? EmployeeId { get; set; }

        // This is the first name of the employee
        public string FirstName { get; set; }

        // This is the last name of the employee
        public string LastName { get; set; }

        // This is the email address of the employee
        public string Email { get; set; }

        // This shows which department the employee works in
        // We use the ID of the department here
        public int DepartmentId { get; set; }
    }
}
