# Contributing to Enterprise Worker Management System

We love your input! We want to make contributing to this project as easy and transparent as possible, whether it's:

- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features
- Becoming a maintainer

## Development Process

We use GitHub to host code, to track issues and feature requests, as well as accept pull requests.

### Pull Request Process

1. **Fork** the repository and create your branch from `main`
2. **Clone** your fork locally
3. **Create** a feature branch: `git checkout -b feature/amazing-feature`
4. **Make** your changes following our coding standards
5. **Add** tests for any new functionality
6. **Ensure** all tests pass
7. **Update** documentation as needed
8. **Commit** with a clear message following our commit conventions
9. **Push** to your fork: `git push origin feature/amazing-feature`
10. **Submit** a pull request

## Code Style and Standards

### C# Coding Conventions

We follow the [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions) with some additions:

#### Naming Conventions

```csharp
// Classes: PascalCase
public class EmployeeService

// Methods: PascalCase
public async Task<Employee> GetEmployeeByIdAsync(int id)

// Properties: PascalCase
public string EmployeeName { get; set; }

// Fields: camelCase with underscore prefix for private
private readonly ILogger _logger;

// Constants: PascalCase
public const int MaxEmployees = 1000;

// Local variables: camelCase
var employeeCount = 10;
```

#### Code Structure

```csharp
// Use explicit access modifiers
public class EmployeeService
{
    private readonly IDbContext _context;

    // Constructor dependency injection
    public EmployeeService(IDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Async methods should have Async suffix
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        try
        {
            return await _context.Employees.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            throw new ServiceException("Failed to retrieve employees", ex);
        }
    }
}
```

### Entity Framework Conventions

```csharp
// Use meaningful navigation properties
public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Navigation properties
    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }
}

// Configure relationships in OnModelCreating
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ProjectEmployee>()
        .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

    modelBuilder.Entity<Employee>()
        .Property(e => e.FirstName)
        .IsRequired()
        .HasMaxLength(100);
}
```

## Testing Standards

### Unit Tests

```csharp
[TestFixture]
public class EmployeeServiceTests
{
    private Mock<IDbContext> _mockContext;
    private EmployeeService _service;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<IDbContext>();
        _service = new EmployeeService(_mockContext.Object);
    }

    [Test]
    public async Task GetEmployeeByIdAsync_ValidId_ReturnsEmployee()
    {
        // Arrange
        var expectedEmployee = new Employee { Id = 1, Name = "John Doe" };
        _mockContext.Setup(x => x.Employees.FindAsync(1))
                   .ReturnsAsync(expectedEmployee);

        // Act
        var result = await _service.GetEmployeeByIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("John Doe"));
    }

    [Test]
    public void GetEmployeeByIdAsync_InvalidId_ThrowsException()
    {
        // Arrange
        _mockContext.Setup(x => x.Employees.FindAsync(-1))
                   .ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        Assert.ThrowsAsync<ServiceException>(
            async () => await _service.GetEmployeeByIdAsync(-1));
    }
}
```

### Test Categories

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test database operations and service interactions
- **UI Tests**: Test form functionality and user interactions

### Test Coverage Requirements

- Minimum 80% code coverage for new features
- 100% coverage for critical business logic
- All public methods must have tests

## Commit Message Conventions

We use [Conventional Commits](https://www.conventionalcommits.org/) specification:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types

- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation only changes
- **style**: Changes that do not affect the meaning of the code
- **refactor**: Code change that neither fixes a bug nor adds a feature
- **perf**: Performance improvements
- **test**: Adding missing tests or correcting existing tests
- **chore**: Changes to the build process or auxiliary tools

### Examples

```
feat(employee): add performance calculation service

Add new service to calculate employee performance metrics
based on completed and overdue tasks.

Closes #123

fix(database): resolve connection timeout issues

Update connection string configuration to increase timeout
and add retry logic for better reliability.

docs(api): update service documentation

Add comprehensive API documentation for EmployeeService
and ProjectService including usage examples.
```

## Database Migrations

### Creating Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove
```

### Migration Guidelines

- **Descriptive names**: Use clear, descriptive migration names
- **Backward compatibility**: Ensure migrations don't break existing data
- **Testing**: Test migrations on a copy of production data
- **Documentation**: Comment complex migration logic

## Error Handling

### Exception Handling Pattern

```csharp
public async Task<Employee> CreateEmployeeAsync(Employee employee)
{
    // Input validation
    if (employee == null)
        throw new ArgumentNullException(nameof(employee));

    if (string.IsNullOrWhiteSpace(employee.Name))
        throw new ArgumentException("Employee name is required", nameof(employee));

    try
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
    catch (DbUpdateException ex)
    {
        _logger.LogError(ex, "Database error creating employee {EmployeeName}", employee.Name);
        throw new ServiceException("Failed to create employee", ex);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error creating employee {EmployeeName}", employee.Name);
        throw;
    }
}
```

### Custom Exceptions

```csharp
public class ServiceException : Exception
{
    public ServiceException(string message) : base(message) { }
    public ServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
```

## Documentation

### XML Documentation

```csharp
/// <summary>
/// Retrieves an employee by their unique identifier.
/// </summary>
/// <param name="employeeId">The unique identifier of the employee.</param>
/// <returns>
/// A task that represents the asynchronous operation.
/// The task result contains the employee if found, otherwise null.
/// </returns>
/// <exception cref="ArgumentException">
/// Thrown when employeeId is less than or equal to zero.
/// </exception>
/// <exception cref="ServiceException">
/// Thrown when a database error occurs during retrieval.
/// </exception>
public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
```

### README Updates

- Update feature lists when adding new functionality
- Include setup instructions for new dependencies
- Add usage examples for new APIs

## Performance Guidelines

### Database Performance

- Use async methods for all database operations
- Implement proper indexing strategies
- Use Include() for related data loading
- Avoid N+1 query problems

### Memory Management

- Dispose of resources properly
- Use `using` statements for disposable objects
- Avoid memory leaks in event handlers

### UI Performance

- Use async/await for long-running operations
- Implement proper error handling in UI
- Show progress indicators for lengthy operations

## Security Considerations

### Input Validation

```csharp
public class EmployeeValidator
{
    public ValidationResult Validate(Employee employee)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(employee.FirstName))
            result.AddError("First name is required");

        if (employee.FirstName?.Length > 100)
            result.AddError("First name cannot exceed 100 characters");

        return result;
    }
}
```

### SQL Injection Prevention

- Always use parameterized queries
- Validate and sanitize input data
- Use Entity Framework's built-in protections

## Release Process

### Version Numbering

We follow [Semantic Versioning](https://semver.org/):

- **MAJOR**: Incompatible API changes
- **MINOR**: Backward compatible functionality additions
- **PATCH**: Backward compatible bug fixes

### Release Checklist

1. Update version numbers in project files
2. Update CHANGELOG.md
3. Ensure all tests pass
4. Update documentation
5. Create release tag
6. Deploy to staging for testing
7. Deploy to production

## Issue Reporting

### Bug Reports

When reporting bugs, please include:

- **Environment**: OS, .NET version, SQL Server version
- **Steps to reproduce**: Clear, numbered steps
- **Expected behavior**: What should happen
- **Actual behavior**: What actually happens
- **Screenshots**: If applicable
- **Error messages**: Full error text and stack traces

### Feature Requests

For feature requests, please include:

- **Problem statement**: What problem does this solve?
- **Proposed solution**: How should it work?
- **Alternatives considered**: Other approaches you've considered
- **Additional context**: Screenshots, mockups, examples

## Getting Help

- **Documentation**: Check the [docs](./docs/) folder
- **Issues**: Search existing [GitHub issues](https://github.com/yourusername/worker-management-system/issues)
- **Discussions**: Use [GitHub Discussions](https://github.com/yourusername/worker-management-system/discussions) for questions

## Code of Conduct

### Our Pledge

We pledge to make participation in our project a harassment-free experience for everyone, regardless of:

- Age, body size, disability, ethnicity, gender identity and expression
- Level of experience, nationality, personal appearance, race, religion
- Sexual identity and orientation

### Our Standards

**Positive behavior includes:**

- Using welcoming and inclusive language
- Being respectful of differing viewpoints and experiences
- Gracefully accepting constructive criticism
- Focusing on what is best for the community

**Unacceptable behavior includes:**

- Trolling, insulting/derogatory comments, and personal attacks
- Public or private harassment
- Publishing others' private information without permission
- Other conduct which could reasonably be considered inappropriate

### Enforcement

Project maintainers are responsible for clarifying standards and will take appropriate action in response to unacceptable behavior.

## Recognition

Contributors will be recognized in:

- **CHANGELOG.md**: Feature contributors listed in release notes
- **README.md**: Major contributors listed in acknowledgments
- **GitHub**: Contributor statistics and commit history

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to making this project better! 🚀
