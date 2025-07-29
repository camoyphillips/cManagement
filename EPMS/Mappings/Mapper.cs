
using EPMS.Models;
using EPMS.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace EPMS.Mappings
{
    public static class Mapper
    {
        // Convert Department to DTO
        public static DepartmentDTO ToDTO(Department dept)
        {
            return new DepartmentDTO
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName
            };
        }

        // Convert Department DTO to entity
        public static Department ToEntity(DepartmentDTO dto)
        {
            return new Department
            {
                DepartmentId = dto.DepartmentId,
                DepartmentName = dto.DepartmentName
            };
        }

        // Convert Employee DTO to entity
        public static Employee ToEntity(EmployeeDTO dto)
        {
            if (dto == null) return null;

            return new Employee
            {
                EmployeeId = dto.EmployeeId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId ?? 0
            };
        }

        // Convert Employee to DTO
        public static EmployeeDTO ToDTO(Employee entity)
        {
            if (entity == null) return null;

            return new EmployeeDTO
            {
                EmployeeId = entity.EmployeeId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                DepartmentId = entity.DepartmentId
            };
        }

        // Update Employee entity with DTO data
        public static void UpdateEntity(EmployeeDTO dto, Employee entity)
        {
            if (dto == null || entity == null) return;

            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Email = dto.Email;
            entity.DepartmentId = dto.DepartmentId ?? 0;
        }

        // Convert Project to DTO
        public static ProjectDTO ToDTO(Project proj)
        {
            return new ProjectDTO
            {
                ProjectId = proj.ProjectId,
                ProjectName = proj.ProjectName,
                Description = proj.Description
            };
        }

        // Convert Project DTO to entity
        public static Project ToEntity(ProjectDTO dto)
        {
            return new Project
            {
                ProjectId = dto.ProjectId,
                ProjectName = dto.ProjectName,
                Description = dto.Description
            };
        }

        // Convert EmployeeProject to DTO
        public static EmployeeProjectDTO ToDTO(EmployeeProject ep)
        {
            return new EmployeeProjectDTO
            {
                EmployeeId = ep.EmployeeId,
                ProjectId = ep.ProjectId
            };
        }

        // Convert EmployeeProject DTO to entity
        public static EmployeeProject ToEntity(EmployeeProjectDTO dto)
        {
            return new EmployeeProject
            {
                EmployeeId = dto.EmployeeId,
                ProjectId = dto.ProjectId
            };
        }

        // Convert list of Departments to DTO list
        public static List<DepartmentDTO> ToDepartmentDTOList(IEnumerable<Department> depts)
        {
            return depts.Select(d => ToDTO(d)).ToList();
        }

        // Convert list of Employees to DTO list
        public static List<EmployeeDTO> ToEmployeeDTOList(IEnumerable<Employee> employees)
        {
            return employees.Select(e => ToDTO(e)).ToList();
        }

        // Convert list of Projects to DTO list
        public static List<ProjectDTO> ToProjectDTOList(IEnumerable<Project> projects)
        {
            return projects.Select(p => ToDTO(p)).ToList();
        }

        // Convert list of EmployeeProjects to DTO list
        public static List<EmployeeProjectDTO> ToEmployeeProjectDTOList(IEnumerable<EmployeeProject> eps)
        {
            return eps.Select(ep => ToDTO(ep)).ToList();
        }
    }
}
