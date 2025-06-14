using WorkerApp.Model;

namespace WorkerApp.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Calisan>> GetAllEmployeesAsync();
        Task<Calisan?> GetEmployeeByIdAsync(int id);
        Task<Calisan> CreateEmployeeAsync(Calisan employee);
        Task<Calisan> UpdateEmployeeAsync(Calisan employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<int> GetCompletedTasksCountAsync(int employeeId);
        Task<int> GetOverdueTasksCountAsync(int employeeId);
        Task<decimal> GetEmployeePerformanceScoreAsync(int employeeId);
    }
}
