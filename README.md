# cManagement

**cManagement** is a modular ASP.NET Core Web Application built to streamline logistics and internal resource management across Drivers, Trucks, Shipments, Employees, Projects, and Departments.

The application is designed with a clean domain-driven structure, integrated CI/CD, and a scalable backend using Entity Framework Core and SQL Server.

---

## MVP Development Goals

The following core goals were established and implemented based on team feedback and milestone reviews:

### Full CRUD for All Base Entities

- **cShipment**: Drivers, Trucks, Shipments  
- **EPMS**: Departments, Employees, Projects  

### New Relationships

- `EmployeeShipment` – a many-to-many join linking Employees to Shipments  
- Additional join models: `DriverShipment`, `EmployeeProject`

### RESTful WebAPI Endpoints

```http
GET    /api/employees/shipments
POST   /api/shipments/{id}/assign-employee
DELETE /api/shipments/{id}/remove-employee


Our project also includes:

List all

Get by ID

Create

Update

Delete

Get associated entities

Add/remove associations

 Razor Views + ViewModels
ShipmentDetailVM: displays Shipment + Assigned Truck + Employees

EmployeeDetailVM: displays Employee + Assigned Projects + Shipments

ProjectDetailVM: displays Project + Employees

Input Validation & UX Feedback
Server-side ModelState.IsValid checks

TempData success/error messages for all form submissions

Core Modules
Entity	Description
Drivers	Manage driver profiles and their assigned shipments
Trucks	Maintain truck fleet information
Shipments	Track logistics operations from origin to destination
Employees	Internal staff managing operations and project participation
Projects	Company-wide initiatives managed and executed by employees
Departments	Department-level organization of employees and resources


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
│   │   └── HomeController.cs
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
│   │   ├── ShipmentDto.cs
│   │   ├── EmployeeDto.cs
│   │   └── AssignmentRequestDto.cs
│   │
│   ├── Models/ViewModels/          
│   │   ├── ShipmentDetailVM.cs
│   │   ├── EmployeeDetailVM.cs
│   │   └── ProjectDetailVM.cs
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
│   │   └── Projects/
│   │
│   ├── wwwroot/                       
│   ├── appsettings.json
│   ├── Program.cs
├── .github/
│   └── workflows/
│       └── dotnet-ci.yml             
│
└── README.md

 Project Status
All base features implemented and verified:

 Code-first models and migrations

 Web API + MVC integration

 Full CRUD with EF Core

 Data annotations for validation

 Relationship management and dropdowns

 GitHub CI workflow with dotnet build and dotnet test

 ViewModels for clear UI rendering

 Team feedback from Project Plan fully incorporated

Next: Begin implementing extra features like authentication, reporting, or real-time tracking.
