cManagement
cManagement is a modular ASP.NET Core Web Application built to streamline logistics and internal resource management across Drivers, Trucks, Shipments, Employees, Projects, and Departments.
The application follows a clean domain-driven architecture, integrates CI/CD with GitHub Actions, and uses Entity Framework Core with SQL Server for a scalable backend.

MVP Development Goals
The following core goals were implemented based on milestone reviews and team feedback:
Full CRUD for All Base Entities
•	cShipment Module: Drivers, Trucks, Shipments
•	EPMS Module: Departments, Employees, Projects
New Relationships
•	EmployeeShipment – many-to-many join between Employees and Shipments
•	DriverShipment – many-to-many join between Drivers and Shipments
•	EmployeeProject – many-to-many join between Employees and Projects
RESTful Web API Endpoints
GET    /api/employees/shipments
POST   /api/shipments/{id}/assign-employee
DELETE /api/shipments/{id}/remove-employee
Additional API Features:
•	List all entities
•	Get by ID
•	Create
•	Update
•	Delete
•	Get associated entities
•	Add/remove associations

Razor Views + ViewModels
•	ShipmentDetailVM – Shipment + Assigned Truck + Employees
•	EmployeeDetailVM – Employee + Assigned Projects + Shipments
•	ProjectDetailVM – Project + Assigned Employees

Core Modules
Entity	Description
Drivers	Manage driver profiles and assigned shipments
Trucks	Maintain truck fleet details
Shipments	Track logistics from origin to destination
Employees	Manage internal staff, roles, and assignments
Projects	Manage company initiatives and team members
Departments	Organize employees by department
Project Structure
cManagement.sln
│
├── cManagement/
│   ├── Controllers/
│   │   ├── DriversController.cs
│   │   ├── TrucksController.cs
│   │   ├── ShipmentsController.cs
│   │   ├── EmployeesController.cs
│   │   ├── ProjectsController.cs
│   │   ├── DepartmentsController.cs
│   │   
│   │
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── DbInitializer.cs
│   │
│   ├── Interfaces/
│   │   ├── IDriverService.cs
│   │   ├── ITruckService.cs
│   │   ├── IShipmentService.cs
│   │   ├── IEmployeeService.cs
│   │   ├── IProjectService.cs
│   │   └── IDepartmentService.cs
│   │
│   ├── Models/
│   │   ├── Driver.cs
│   │   ├── Truck.cs
│   │   ├── Shipment.cs
│   │   ├── Employee.cs
│   │   ├── Project.cs
│   │   ├── Department.cs
│   │   ├── DriverShipment.cs
│   │   ├── EmployeeProject.cs
│   │   └── EmployeeShipment.cs
│   │
│   ├── Models/Dtos/
│   │   ├── DriverDto.cs
│   │   ├── TruckDto.cs
│   │   ├── ShipmentDto.cs
│   │   ├── EmployeeDto.cs
│   │   ├── ProjectDto.cs
│   │   
│   │
│   ├── Models/ViewModels/
│   │   ├── ShipmentDetailVM.cs
│   │   ├── EmployeeDetailVM.cs
│   │   ├── ProjectDetailVM.cs
│   │   └── TruckList.cs
│   │
│   ├── Services/
│   │   ├── DriverService.cs
│   │   ├── TruckService.cs
│   │   ├── ShipmentService.cs
│   │   ├── EmployeeService.cs
│   │   ├── ProjectService.cs
│   │   └── DepartmentService.cs
│   │
│   ├── Views/
│   │   ├── Shared/
│   │   ├── Home/
│   │   ├── Shipments/
│   │   ├── Employees/
│   │   ├── Drivers/
│   │   ├── Projects/
│   │   ├── Departments/
│   │   └── TruckPage/
│   │
│   ├── wwwroot/
│   ├── appsettings.json
│   ├── Program.cs
│
├── .github/
│   └── workflows/
│       └── dotnet-ci.yml
│
└── README.md

Technology Stack
•	Framework: ASP.NET Core 8.0 MVC 
•	Database: SQL Server + EF Core (Code-First)
•	Frontend: Razor Views
•	CI/CD: GitHub Actions 
•	Patterns: Dependency Injection
•	Validation: Data Annotations 
•	Version Control: Git + GitHub

 Project Status
 Features implemented and verified:
•	Code-first models + migrations
•	Web API + MVC integration
•	Full CRUD for all entities
•	Validation with data annotations
•	Many-to-many relationship management
•	ViewModels for clean UI rendering
•	CI/CD pipeline active and passing
