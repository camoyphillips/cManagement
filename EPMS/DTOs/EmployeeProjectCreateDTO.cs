namespace EPMS.DTOs
{
    // This class is used when we want to assign a project to an employee
    // It only stores the IDs that connect an employee to a project
    public class EmployeeProjectCreateDTO
    {
        // This is the ID of the employee
        // It helps us know which employee is getting the project
        public int EmployeeId { get; set; }

        // This is the ID of the project
        // It helps us know which project is being given to the employee
        public int ProjectId { get; set; }
    }
}
