using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Dtos.Requests;
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

        public async Task<(IEnumerable<ActivityLog> Items, int TotalCount, int CreateCount, int UpdateCount, int DeleteCount)> GetFilteredActivityLogsAsync(ActivityLogParamsDto filter)
        {
            var query = _context.ActivityLogs
                .Include(x => x.User)
                .ThenInclude(u => u.Role)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.ToLower();
                query = query.Where(x =>
                    (x.Details != null && x.Details.ToLower().Contains(search)) ||
                    (x.Entity != null && x.Entity.ToLower().Contains(search)) ||
                    (x.User != null && x.User.FirstName.ToLower().Contains(search)));
            }

            // ── التعديل السحري: دعم الهمزات والمرادفات للحروف العربية ──
            if (!string.IsNullOrWhiteSpace(filter.Action) && filter.Action.ToLower() != "all")
            {
                var act = filter.Action.ToLower();

                if (act == "create")
                    query = query.Where(x => x.Action != null && (
                        x.Action.ToLower().Contains("create") || x.Action.ToLower().Contains("add") ||
                        x.Action.Contains("إضاف") || x.Action.Contains("اضاف") ||
                        x.Action.Contains("إنشا") || x.Action.Contains("انشا")));
                else if (act == "update")
                    query = query.Where(x => x.Action != null && (
                        x.Action.ToLower().Contains("update") || x.Action.ToLower().Contains("edit") ||
                        x.Action.Contains("تعديل") || x.Action.Contains("تحديث")));
                else if (act == "delete")
                    query = query.Where(x => x.Action != null && (
                        x.Action.ToLower().Contains("delete") || x.Action.ToLower().Contains("remove") ||
                        x.Action.Contains("حذف") || x.Action.Contains("إلغاء") || x.Action.Contains("الغاء")));
                else if (act == "login")
                    query = query.Where(x => x.Action != null && (
                        x.Action.ToLower().Contains("login") || x.Action.ToLower().Contains("auth") ||
                        x.Action.Contains("تسجيل") || x.Action.Contains("دخول")));
                else
                    query = query.Where(x => x.Action != null && x.Action.ToLower() == act);
            }

            if (!string.IsNullOrWhiteSpace(filter.Entity) && filter.Entity.ToLower() != "all")
                query = query.Where(x => x.Entity != null && x.Entity.ToLower() == filter.Entity.ToLower());

            if (filter.DateFrom.HasValue)
                query = query.Where(x => x.Timestamp >= filter.DateFrom.Value);

            if (filter.DateTo.HasValue)
            {
                var dateTo = filter.DateTo.Value.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.Timestamp <= dateTo);
            }

            var totalCount = await query.CountAsync();

            // ── تحديث الإحصائيات لتطابق نفس الكلمات بالضبط ──
            var actionTypes = await query.Select(x => x.Action != null ? x.Action.ToLower() : "").ToListAsync();

            var createCount = actionTypes.Count(a => a.Contains("create") || a.Contains("add") || a.Contains("إضاف") || a.Contains("اضاف") || a.Contains("إنشا") || a.Contains("انشا"));
            var updateCount = actionTypes.Count(a => a.Contains("update") || a.Contains("edit") || a.Contains("تعديل") || a.Contains("تحديث"));
            var deleteCount = actionTypes.Count(a => a.Contains("delete") || a.Contains("remove") || a.Contains("حذف") || a.Contains("إلغاء") || a.Contains("الغاء"));

            var items = await query
                .OrderByDescending(x => x.Timestamp)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (items, totalCount, createCount, updateCount, deleteCount);
        }
    }
}