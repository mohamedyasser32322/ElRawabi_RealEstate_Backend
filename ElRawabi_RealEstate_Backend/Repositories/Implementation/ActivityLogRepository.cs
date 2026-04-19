using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<ActivityLog> _dbSet;

        public ActivityLogRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<ActivityLog>();
        }

        public async Task<IEnumerable<ActivityLog>> GetAllActivityLogsAsync()
        {
            return await _context.ActivityLogs
                .Include(x => x.User)
                    .ThenInclude(u => u.Role)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();
        }
        public async Task<ActivityLog?> GetActivityLogByIdAsync(int id)
            => await _context.ActivityLogs
                .Include(x => x.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public async Task AddActivityLogAsync(ActivityLog activityLog) => await _dbSet.AddAsync(activityLog);
        public void UpdateActivityLog(ActivityLog activityLog) => _dbSet.Update(activityLog);
        public void DeleteActivityLog(ActivityLog activityLog) => _dbSet.Remove(activityLog);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<ActivityLog>> GetActivityLogsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(al => al.UserId == userId).ToListAsync();
        }
    }
}