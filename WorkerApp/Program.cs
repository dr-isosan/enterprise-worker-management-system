using WorkerApp.Model;
using WorkerApp.Services;

namespace WorkerApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                // Create database context and services
                var context = new AppDBContext();
                var employeeService = new EmployeeService(context);
                var projectService = new ProjectService(context);

                // Create and run main form
                Application.Run(new Form(context, employeeService, projectService));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
