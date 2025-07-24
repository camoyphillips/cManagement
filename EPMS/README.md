
#  Employee Project Management System (EPMS)

## What is EPMS?

EPMS is a simple Content Management System (CMS) built using ASP.NET Core MVC, Entity Framework, and C#. It helps administrators manage:

- Departments
- Employees
- Projects
- Project Assignments (M-M)

It’s a CRUD-based system designed as a Passion Project to learn how to build full-stack web applications using ASP.NET Core.

---

## Features

- Employees: Add, view, edit, and delete employees. Each belongs to a department and can work on multiple projects.
- Departments: Manage department records.
- Projects: Manage project details.
- Project Allocations: Assign employees to projects (Many-to-Many).

---

## ️ Database Design

- Department (1-M → Employee)
- Employee ↔ Project (M-M via `EmployeeProject`)
- All entities are created using code-first migrations.
---

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- LINQ
- Razor Views
- C#
- SQL Server
- Swagger 

---

##  How to Run It

1. Clone this repo 
2. Open in Visual Studio 2022 or newer
3. Apply Migrations & Update Database
4. Run the App

---

## Project Structure


EPMS/
├── Controllers/             # MVC + API controllers
├── DTOs/                   # Data Transfer Objects
├── Models/                 # Entity models
├── Services/               # Services and interfaces
├── Views/                  # Razor pages
├── Data/                   # DbContext + Migrations
└── Mappings/               # Mapper logic


---

## Testing the API

Use Swagger UI:
http://localhost:<port>/swagger

You can test:
- `GET /api/EmployeesApi`
- `POST /api/DepartmentsApi`
- etc.

---

##  Goals Achieved

- 1-M and M-M relationships
- CRUD operations for all entities
- LINQ + DTO usage
- MVC + API + Swagger

---


##  Author
- Saahil Sayed

