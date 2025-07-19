# cManagement

**cManagement** is a modular ASP.NET Core Web Application built to streamline logistics and internal resource management across Drivers, Trucks, Shipments, Employees, Projects, and Departments.

The application is designed with a clean domain-driven structure, integrated CI/CD, and a scalable backend using Entity Framework Core and SQL Server.

---

##  Core Modules

| Entity       | Description                                                  |
|--------------|--------------------------------------------------------------|
| Drivers      | Manage driver profiles and their assigned shipments          |
| Trucks       | Maintain truck fleet information                             |
| Shipments    | Track logistics operations from origin to destination        |
| Employees    | Internal staff managing operations and project participation |
| Projects     | Company-wide initiatives managed and executed by employees   |
| Departments  | Department-level organization of employees and resources     |

---

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
│   └── Startup.cs
│
├── cManagement.Tests/                 
│   ├── cManagement.Tests.csproj
│   ├── ShipmentServiceTests.cs
│   ├── DriverServiceTests.cs
│   ├── EmployeeServiceTests.cs
│   └── ProjectAssignmentTests.cs
│
├── .github/
│   └── workflows/
│       └── dotnet-ci.yml             # GitHub Actions CI Pipeline
│
└── README.md
