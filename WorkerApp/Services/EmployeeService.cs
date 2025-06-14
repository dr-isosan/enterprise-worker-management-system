using Microsoft.EntityFrameworkCore;
using WorkerApp.Model;

namespace WorkerApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDBContext _context;

        public EmployeeService(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Calisan>> GetAllEmployeesAsync()
        {
            try
            {
                return await _context.Calisanlar
                    .Include(c => c.ProjeCalisanlar)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception in a real application
                throw new InvalidOperationException("Error retrieving employees", ex);
            }
        }

        public async Task<Calisan?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return await _context.Calisanlar
                    .Include(c => c.ProjeCalisanlar)
                    .FirstOrDefaultAsync(c => c.CalisanNo == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving employee with ID {id}", ex);
            }
        }

        public async Task<Calisan> CreateEmployeeAsync(Calisan employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (string.IsNullOrWhiteSpace(employee.CalisanAdi))
                throw new ArgumentException("Employee first name is required", nameof(employee));

            if (string.IsNullOrWhiteSpace(employee.CalisanSoyadi))
                throw new ArgumentException("Employee last name is required", nameof(employee));

            try
            {
                _context.Calisanlar.Add(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating employee", ex);
            }
        }

        public async Task<Calisan> UpdateEmployeeAsync(Calisan employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            try
            {
                var existingEmployee = await _context.Calisanlar
                    .FindAsync(employee.CalisanNo);

                if (existingEmployee == null)
                    throw new InvalidOperationException($"Employee with ID {employee.CalisanNo} not found");

                existingEmployee.CalisanAdi = employee.CalisanAdi;
                existingEmployee.CalisanSoyadi = employee.CalisanSoyadi;
                existingEmployee.TamamlananGorevSayisi = employee.TamamlananGorevSayisi;
                existingEmployee.GecikenGorevSayisi = employee.GecikenGorevSayisi;

                await _context.SaveChangesAsync();
                return existingEmployee;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating employee", ex);
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _context.Calisanlar.FindAsync(id);
                if (employee == null)
                    return false;

                _context.Calisanlar.Remove(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting employee with ID {id}", ex);
            }
        }

        public async Task<int> GetCompletedTasksCountAsync(int employeeId)
        {
            try
            {
                return await _context.Gorevler
                    .Where(g => g.CalisanNo == employeeId && g.Durum == "Completed")
                    .CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error getting completed tasks count for employee {employeeId}", ex);
            }
        }

        public async Task<int> GetOverdueTasksCountAsync(int employeeId)
        {
            try
            {
                return await _context.Gorevler
                    .Where(g => g.CalisanNo == employeeId &&
                               g.BitisTarihi < DateTime.Now &&
                               g.Durum != "Completed")
                    .CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error getting overdue tasks count for employee {employeeId}", ex);
            }
        }

        public async Task<decimal> GetEmployeePerformanceScoreAsync(int employeeId)
        {
            try
            {
                var completedTasks = await GetCompletedTasksCountAsync(employeeId);
                var overdueTasks = await GetOverdueTasksCountAsync(employeeId);
                var totalTasks = completedTasks + overdueTasks;

                if (totalTasks == 0)
                    return 0;

                // Performance score: (completed tasks / total tasks) * 100
                return (decimal)completedTasks / totalTasks * 100;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating performance score for employee {employeeId}", ex);
            }
        }
    }
}
