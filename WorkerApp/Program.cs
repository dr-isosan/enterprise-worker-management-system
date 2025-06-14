using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WorkerApp.Model;
using WorkerApp.Services;

namespace WorkerApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Configures dependency injection, logging, and starts the Windows Forms application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/worker-app-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting Worker Management Application");

                // Configure services
                var services = new ServiceCollection();
                ConfigureServices(services);

                var serviceProvider = services.BuildServiceProvider();

                // Configure Windows Forms
                ApplicationConfiguration.Initialize();

                // Create and run main form with dependency injection
                using var scope = serviceProvider.CreateScope();
                var mainForm = scope.ServiceProvider.GetRequiredService<Form>();

                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
                MessageBox.Show($"A critical error occurred: {ex.Message}", "Application Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Configures dependency injection services for the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        private static void ConfigureServices(ServiceCollection services)
        {
            // Configure Entity Framework
            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(GetConnectionString(), sqlOptions =>
                {
                    sqlOptions.CommandTimeout(30);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });

                // Enable sensitive data logging only in debug mode
                #if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                #endif
            });

            // Configure logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();

                #if DEBUG
                builder.SetMinimumLevel(LogLevel.Debug);
                #else
                builder.SetMinimumLevel(LogLevel.Information);
                #endif
            });

            // Register services
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IProjectService, ProjectService>();

            // Register forms
            services.AddTransient<Form>();

            // Register additional services
            services.AddSingleton<IConfiguration>(provider =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Production"}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
                return config;
            });
        }

        /// <summary>
        /// Gets the database connection string from configuration or returns a default.
        /// </summary>
        /// <returns>The database connection string.</returns>
        private static string GetConnectionString()
        {
            // Try to get from environment variable first (for production)
            var connectionString = Environment.GetEnvironmentVariable("WORKER_DB_CONNECTION_STRING");

            if (!string.IsNullOrEmpty(connectionString))
            {
                Log.Information("Using connection string from environment variable");
                return connectionString;
            }

            // Default connection string for development
            var defaultConnectionString = "Data Source=DESKTOP-IA05O5S\\SQLEXPRESS;Initial Catalog=WorkerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            Log.Information("Using default development connection string");
            return defaultConnectionString;
        }
    }

    /// <summary>
    /// Configuration interface for dependency injection.
    /// </summary>
    public interface IConfiguration
    {
        string GetConnectionString(string name);
        T GetValue<T>(string key);
    }

    /// <summary>
    /// Basic configuration implementation.
    /// </summary>
    public class Configuration : IConfiguration
    {
        private readonly Dictionary<string, string> _settings;

        public Configuration()
        {
            _settings = new Dictionary<string, string>();
        }

        public string GetConnectionString(string name)
        {
            return _settings.TryGetValue($"ConnectionStrings:{name}", out var value) ? value : string.Empty;
        }

        public T GetValue<T>(string key)
        {
            if (_settings.TryGetValue(key, out var value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            return default(T);
        }
    }
}
