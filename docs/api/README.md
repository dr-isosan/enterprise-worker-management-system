# API Documentation

## Employee Service

### IEmployeeService Interface

The Employee Service provides comprehensive functionality for managing employee data and performance metrics.

#### Methods

##### GetAllEmployeesAsync()

```csharp
Task<IEnumerable<Calisan>> GetAllEmployeesAsync()
```

- **Description**: Retrieves all employees with their project relationships
- **Returns**: Collection of all employees
- **Throws**: `InvalidOperationException` if database error occurs

##### GetEmployeeByIdAsync(int id)

```csharp
Task<Calisan?> GetEmployeeByIdAsync(int id)
```

- **Description**: Retrieves a specific employee by ID
- **Parameters**:
  - `id`: Employee identifier
- **Returns**: Employee object or null if not found
- **Throws**: `InvalidOperationException` if database error occurs

##### CreateEmployeeAsync(Calisan employee)

```csharp
Task<Calisan> CreateEmployeeAsync(Calisan employee)
```

- **Description**: Creates a new employee record
- **Parameters**:
  - `employee`: Employee object to create
- **Returns**: Created employee with assigned ID
- **Throws**:
  - `ArgumentNullException` if employee is null
  - `ArgumentException` if required fields are missing
  - `InvalidOperationException` if database error occurs

##### UpdateEmployeeAsync(Calisan employee)

```csharp
Task<Calisan> UpdateEmployeeAsync(Calisan employee)
```

- **Description**: Updates an existing employee record
- **Parameters**:
  - `employee`: Employee object with updated data
- **Returns**: Updated employee object
- **Throws**:
  - `ArgumentNullException` if employee is null
  - `InvalidOperationException` if employee not found or database error occurs

##### DeleteEmployeeAsync(int id)

```csharp
Task<bool> DeleteEmployeeAsync(int id)
```

- **Description**: Deletes an employee record
- **Parameters**:
  - `id`: Employee identifier
- **Returns**: True if deleted successfully, false if not found
- **Throws**: `InvalidOperationException` if database error occurs

##### GetCompletedTasksCountAsync(int employeeId)

```csharp
Task<int> GetCompletedTasksCountAsync(int employeeId)
```

- **Description**: Gets the count of completed tasks for an employee
- **Parameters**:
  - `employeeId`: Employee identifier
- **Returns**: Number of completed tasks
- **Throws**: `InvalidOperationException` if database error occurs

##### GetOverdueTasksCountAsync(int employeeId)

```csharp
Task<int> GetOverdueTasksCountAsync(int employeeId)
```

- **Description**: Gets the count of overdue tasks for an employee
- **Parameters**:
  - `employeeId`: Employee identifier
- **Returns**: Number of overdue tasks
- **Throws**: `InvalidOperationException` if database error occurs

##### GetEmployeePerformanceScoreAsync(int employeeId)

```csharp
Task<decimal> GetEmployeePerformanceScoreAsync(int employeeId)
```

- **Description**: Calculates performance score based on completed vs total tasks
- **Parameters**:
  - `employeeId`: Employee identifier
- **Returns**: Performance score as percentage (0-100)
- **Throws**: `InvalidOperationException` if database error occurs

## Project Service

### IProjectService Interface

The Project Service provides functionality for managing projects, tracking progress, and calculating metrics.

#### Methods

##### GetAllProjectsAsync()

```csharp
Task<IEnumerable<Proje>> GetAllProjectsAsync()
```

- **Description**: Retrieves all projects with their relationships
- **Returns**: Collection of all projects
- **Throws**: `InvalidOperationException` if database error occurs

##### GetProjectByIdAsync(int id)

```csharp
Task<Proje?> GetProjectByIdAsync(int id)
```

- **Description**: Retrieves a specific project by ID
- **Parameters**:
  - `id`: Project identifier
- **Returns**: Project object or null if not found
- **Throws**: `InvalidOperationException` if database error occurs

##### CreateProjectAsync(Proje project)

```csharp
Task<Proje> CreateProjectAsync(Proje project)
```

- **Description**: Creates a new project record
- **Parameters**:
  - `project`: Project object to create
- **Returns**: Created project with assigned ID
- **Throws**:
  - `ArgumentNullException` if project is null
  - `ArgumentException` if validation fails
  - `InvalidOperationException` if database error occurs

##### GetOverdueProjectsAsync()

```csharp
Task<IEnumerable<Proje>> GetOverdueProjectsAsync()
```

- **Description**: Retrieves all projects that are past their end date
- **Returns**: Collection of overdue projects
- **Throws**: `InvalidOperationException` if database error occurs

##### GetActiveProjectsAsync()

```csharp
Task<IEnumerable<Proje>> GetActiveProjectsAsync()
```

- **Description**: Retrieves all currently active projects
- **Returns**: Collection of active projects
- **Throws**: `InvalidOperationException` if database error occurs

##### CalculateProjectDelayAsync(int projectId)

```csharp
Task<int> CalculateProjectDelayAsync(int projectId)
```

- **Description**: Calculates the number of days a project is delayed
- **Parameters**:
  - `projectId`: Project identifier
- **Returns**: Number of delay days (0 if not delayed)
- **Throws**: `InvalidOperationException` if project not found or database error occurs

##### GetProjectCompletionPercentageAsync(int projectId)

```csharp
Task<decimal> GetProjectCompletionPercentageAsync(int projectId)
```

- **Description**: Calculates project completion percentage based on completed tasks
- **Parameters**:
  - `projectId`: Project identifier
- **Returns**: Completion percentage (0-100)
- **Throws**: `InvalidOperationException` if database error occurs

## Error Handling

All service methods implement comprehensive error handling:

- **Validation Errors**: `ArgumentNullException`, `ArgumentException`
- **Business Logic Errors**: `InvalidOperationException`
- **Database Errors**: Wrapped in `InvalidOperationException` with inner exception details

## Usage Examples

### Employee Management

```csharp
// Create employee service
var employeeService = new EmployeeService(dbContext);

// Get all employees
var employees = await employeeService.GetAllEmployeesAsync();

// Create new employee
var newEmployee = new Calisan
{
    CalisanAdi = "John",
    CalisanSoyadi = "Doe"
};
var created = await employeeService.CreateEmployeeAsync(newEmployee);

// Get performance score
var score = await employeeService.GetEmployeePerformanceScoreAsync(created.CalisanNo);
```

### Project Management

```csharp
// Create project service
var projectService = new ProjectService(dbContext);

// Get overdue projects
var overdueProjects = await projectService.GetOverdueProjectsAsync();

// Calculate project completion
var completionRate = await projectService.GetProjectCompletionPercentageAsync(projectId);
```
