namespace EPMS.DTOs
{
    // This class is for showing which employee is working on which project
    // It is used when we want to display the info

    public class EmployeeProjectDTO
    {
        // This is the ID of the employee
        public int EmployeeId { get; set; }

        // This is the ID of the project
        public int ProjectId { get; set; }

        // The first name of the employee
        public string FirstName { get; set; }

        // The last name of the employee
        public string LastName { get; set; }

        // The name of the project assigned to the employee
        public string ProjectName { get; set; }

        // This joins first name and last name to show the full name
        public string EmployeeName => FirstName + " " + LastName;
    }
}
