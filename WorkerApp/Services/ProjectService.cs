using Microsoft.EntityFrameworkCore;
using WorkerApp.Model;

namespace WorkerApp.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDBContext _context;

        public ProjectService(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Proje>> GetAllProjectsAsync()
        {
            try
            {
                return await _context.Projeler
                    .Include(p => p.ProjeCalisanlar)
                    .Include(p => p.Gorevler)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving projects", ex);
            }
        }

        public async Task<Proje?> GetProjectByIdAsync(int id)
        {
            try
            {
                return await _context.Projeler
                    .Include(p => p.ProjeCalisanlar)
                    .Include(p => p.Gorevler)
                    .FirstOrDefaultAsync(p => p.ProjeNo == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving project with ID {id}", ex);
            }
        }

        public async Task<Proje> CreateProjectAsync(Proje project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            if (string.IsNullOrWhiteSpace(project.ProjeAdi))
                throw new ArgumentException("Project name is required", nameof(project));

            if (project.BaslangicTarihi >= project.BitisTarihi)
                throw new ArgumentException("Start date must be before end date", nameof(project));

            try
            {
                _context.Projeler.Add(project);
                await _context.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating project", ex);
            }
        }

        public async Task<Proje> UpdateProjectAsync(Proje project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            try
            {
                var existingProject = await _context.Projeler.FindAsync(project.ProjeNo);
                if (existingProject == null)
                    throw new InvalidOperationException($"Project with ID {project.ProjeNo} not found");

                existingProject.ProjeAdi = project.ProjeAdi;
                existingProject.BaslangicTarihi = project.BaslangicTarihi;
                existingProject.BitisTarihi = project.BitisTarihi;
                existingProject.GecikmeMiktari = project.GecikmeMiktari;

                await _context.SaveChangesAsync();
                return existingProject;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating project", ex);
            }
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            try
            {
                var project = await _context.Projeler.FindAsync(id);
                if (project == null)
                    return false;

                _context.Projeler.Remove(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting project with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<Proje>> GetOverdueProjectsAsync()
        {
            try
            {
                return await _context.Projeler
                    .Where(p => p.BitisTarihi < DateTime.Now)
                    .Include(p => p.Gorevler)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving overdue projects", ex);
            }
        }

        public async Task<IEnumerable<Proje>> GetActiveProjectsAsync()
        {
            try
            {
                return await _context.Projeler
                    .Where(p => p.BaslangicTarihi <= DateTime.Now && p.BitisTarihi >= DateTime.Now)
                    .Include(p => p.Gorevler)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving active projects", ex);
            }
        }

        public async Task<int> CalculateProjectDelayAsync(int projectId)
        {
            try
            {
                var project = await _context.Projeler.FindAsync(projectId);
                if (project == null)
                    throw new InvalidOperationException($"Project with ID {projectId} not found");

                if (DateTime.Now <= project.BitisTarihi)
                    return 0; // No delay

                return (DateTime.Now - project.BitisTarihi).Days;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating delay for project {projectId}", ex);
            }
        }

        public async Task<decimal> GetProjectCompletionPercentageAsync(int projectId)
        {
            try
            {
                var totalTasks = await _context.Gorevler
                    .Where(g => g.ProjeNo == projectId)
                    .CountAsync();

                if (totalTasks == 0)
                    return 0;

                var completedTasks = await _context.Gorevler
                    .Where(g => g.ProjeNo == projectId && g.Durum == "Completed")
                    .CountAsync();

                return (decimal)completedTasks / totalTasks * 100;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating completion percentage for project {projectId}", ex);
            }
        }
    }
}
