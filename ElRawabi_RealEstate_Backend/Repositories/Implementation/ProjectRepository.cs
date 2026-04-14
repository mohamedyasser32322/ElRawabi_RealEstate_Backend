using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<Project> _dbSet;

        public ProjectRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Project>();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync() => await _dbSet.ToListAsync();
        public async Task<Project?> GetProjectByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddProjectAsync(Project project) => await _dbSet.AddAsync(project);
        public void UpdateProject(Project project) => _dbSet.Update(project);
        public void DeleteProject(Project project) => _dbSet.Remove(project); // Consider soft delete logic here if applicable
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<Project?> GetProjectWithBuildingsAsync(int projectId)
        {
            return await _dbSet.Include(p => p.Buildings).FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
        {
            return await _dbSet.Where(p => p.AvailableUnits > 0).ToListAsync();
        }
    }
}


