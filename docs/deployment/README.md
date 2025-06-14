# Deployment Guide

## Production Deployment

### Prerequisites

1. **Windows Server 2019/2022** or **Windows 10/11**
2. **.NET 8.0 Runtime** (Desktop)
3. **SQL Server 2019+** or **SQL Server Express**
4. **IIS** (if web interface is added later)

### Installation Steps

#### 1. Server Preparation

```powershell
# Enable Windows Features (if needed)
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServer
Enable-WindowsOptionalFeature -Online -FeatureName IIS-CommonHttpFeatures
Enable-WindowsOptionalFeature -Online -FeatureName IIS-HttpErrors
Enable-WindowsOptionalFeature -Online -FeatureName IIS-HttpLogging
Enable-WindowsOptionalFeature -Online -FeatureName IIS-RequestFiltering
```

#### 2. Install .NET 8.0 Runtime

Download and install from: <https://dotnet.microsoft.com/download/dotnet/8.0>

```powershell
# Verify installation
dotnet --info
```

#### 3. SQL Server Setup

```sql
-- Create database
CREATE DATABASE WorkerDB;
GO

-- Create application user
CREATE LOGIN WorkerAppUser WITH PASSWORD = 'SecurePassword123!';
GO

USE WorkerDB;
CREATE USER WorkerAppUser FOR LOGIN WorkerAppUser;
GO

-- Grant permissions
ALTER ROLE db_datareader ADD MEMBER WorkerAppUser;
ALTER ROLE db_datawriter ADD MEMBER WorkerAppUser;
ALTER ROLE db_ddladmin ADD MEMBER WorkerAppUser;
GO
```

#### 4. Application Deployment

```bash
# Build for production
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish

# Or self-contained
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish-standalone
```

#### 5. Configuration

Update `appsettings.production.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=PROD-SQL-SERVER;Database=WorkerDB;User Id=WorkerAppUser;Password=SecurePassword123!;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

#### 6. Database Migration

```bash
# Update database with migrations
dotnet ef database update --connection "YourProductionConnectionString"
```

### Security Considerations

#### 1. Connection String Security

- Store connection strings in secure configuration
- Use Windows Authentication when possible
- Encrypt sensitive configuration sections

```xml
<!-- In app.config for encryption -->
<configuration>
  <connectionStrings configProtectionProvider="RsaProtectedConfigurationProvider">
    <EncryptedData>
      <!-- Encrypted connection string -->
    </EncryptedData>
  </connectionStrings>
</configuration>
```

#### 2. Application Security

- Run application with least privilege account
- Configure Windows Firewall rules
- Enable SQL Server encryption
- Use SSL/TLS for any network communication

#### 3. File System Permissions

```powershell
# Set appropriate permissions on application folder
icacls "C:\Apps\WorkerManagement" /grant "IIS_IUSRS:(OI)(CI)R"
icacls "C:\Apps\WorkerManagement\Logs" /grant "IIS_IUSRS:(OI)(CI)F"
```

### Monitoring and Logging

#### 1. Event Log Configuration

```csharp
// Add to Program.cs
services.AddLogging(builder =>
{
    builder.AddEventLog(settings =>
    {
        settings.SourceName = "WorkerManagementApp";
        settings.LogName = "Application";
    });
});
```

#### 2. Performance Counters

```csharp
// Custom performance counters
public class PerformanceCounterService
{
    private readonly PerformanceCounter _employeeOperationsCounter;
    private readonly PerformanceCounter _projectOperationsCounter;

    public PerformanceCounterService()
    {
        _employeeOperationsCounter = new PerformanceCounter(
            "WorkerApp", "Employee Operations", false);
        _projectOperationsCounter = new PerformanceCounter(
            "WorkerApp", "Project Operations", false);
    }
}
```

#### 3. Health Checks

```csharp
// Add health checks
services.AddHealthChecks()
    .AddSqlServer(connectionString)
    .AddDbContextCheck<AppDBContext>();
```

### Backup and Recovery

#### 1. Database Backup Strategy

```sql
-- Full backup (weekly)
BACKUP DATABASE WorkerDB
TO DISK = 'C:\Backups\WorkerDB_Full.bak'
WITH FORMAT, INIT;

-- Differential backup (daily)
BACKUP DATABASE WorkerDB
TO DISK = 'C:\Backups\WorkerDB_Diff.bak'
WITH DIFFERENTIAL, FORMAT, INIT;

-- Transaction log backup (every 15 minutes)
BACKUP LOG WorkerDB
TO DISK = 'C:\Backups\WorkerDB_Log.trn'
WITH FORMAT, INIT;
```

#### 2. Application Backup

```powershell
# Automated backup script
$source = "C:\Apps\WorkerManagement"
$destination = "C:\Backups\App\WorkerManagement_$(Get-Date -Format 'yyyyMMdd')"
Robocopy $source $destination /E /XD bin obj .vs
```

### Performance Optimization

#### 1. Database Performance

```sql
-- Add indexes for better performance
CREATE INDEX IX_Gorevler_CalisanNo ON Gorevler(CalisanNo);
CREATE INDEX IX_Gorevler_ProjeNo ON Gorevler(ProjeNo);
CREATE INDEX IX_Gorevler_Durum ON Gorevler(Durum);
CREATE INDEX IX_ProjeCalisanlar_ProjeNo ON ProjeCalisanlar(ProjeNo);
CREATE INDEX IX_ProjeCalisanlar_CalisanNo ON ProjeCalisanlar(CalisanNo);

-- Update statistics
UPDATE STATISTICS Calisanlar;
UPDATE STATISTICS Projeler;
UPDATE STATISTICS Gorevler;
```

#### 2. Application Performance

```csharp
// Configure Entity Framework for production
services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(30);
        sqlOptions.EnableRetryOnFailure(3);
    });

    // Disable sensitive data logging in production
    options.EnableSensitiveDataLogging(false);
    options.EnableDetailedErrors(false);
});
```

### Scaling Considerations

#### 1. Horizontal Scaling

- Use SQL Server Always On for database high availability
- Implement read replicas for reporting queries
- Consider load balancing for multiple application instances

#### 2. Vertical Scaling

- Monitor CPU and memory usage
- Optimize database queries with proper indexing
- Implement caching strategies

### Troubleshooting

#### Common Issues

1. **Connection String Issues**

   ```
   Error: "Login failed for user"
   Solution: Verify SQL credentials and network connectivity
   ```

2. **Migration Issues**

   ```
   Error: "Database migration failed"
   Solution: Check database permissions and migration scripts
   ```

3. **Performance Issues**

   ```
   Error: Slow queries
   Solution: Check database indexes and query execution plans
   ```

#### Diagnostic Commands

```powershell
# Check .NET installation
dotnet --info

# Check application status
Get-Process | Where-Object {$_.Name -like "*WorkerApp*"}

# Check SQL Server connections
netstat -an | findstr 1433

# Check event logs
Get-EventLog -LogName Application -Source "WorkerManagementApp" -Newest 50
```

### Maintenance

#### Regular Maintenance Tasks

1. **Weekly**
   - Review error logs
   - Check database backup status
   - Monitor disk space usage

2. **Monthly**
   - Update statistics
   - Rebuild indexes
   - Review performance metrics

3. **Quarterly**
   - Security patch updates
   - Performance optimization review
   - Disaster recovery testing
