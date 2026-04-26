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
                .AsNoTracking()
                .Include(x => x.User)
                    .ThenInclude(u => u.Role)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();
        }

        public async Task<ActivityLog?> GetActivityLogByIdAsync(int id)
            => await _context.ActivityLogs
                .AsNoTracking()
                .Include(x => x.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task AddActivityLogAsync(ActivityLog activityLog) => await _dbSet.AddAsync(activityLog);
        public void UpdateActivityLog(ActivityLog activityLog) => _dbSet.Update(activityLog);
        public void DeleteActivityLog(ActivityLog activityLog) => _dbSet.Remove(activityLog);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<ActivityLog>> GetActivityLogsByUserIdAsync(int userId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(al => al.UserId == userId)
                .ToListAsync();
        }

        public async Task<(IEnumerable<ActivityLog> Items, int TotalCount, int CreateCount, int UpdateCount, int DeleteCount)> GetFilteredActivityLogsAsync(ActivityLogParamsDto filter)
        {
            var query = _context.ActivityLogs
                .AsNoTracking()
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
                query = query.Where(x => x.Timestamp <= filter.DateTo.Value.AddDays(1).AddTicks(-1));

            // ✅ Query 1: جيب الـ actions بس عشان تعد
            var allActions = await query
                .Select(x => x.Action ?? "")
                .ToListAsync();

            var totalCount = allActions.Count;
            var createCount = allActions.Count(a =>
                a.Contains("إضاف") || a.Contains("اضاف") ||
                a.Contains("إنشا") || a.Contains("انشا") ||
                a.ToLower().Contains("create") || a.ToLower().Contains("add"));
            var updateCount = allActions.Count(a =>
                a.Contains("تعديل") || a.Contains("تحديث") ||
                a.ToLower().Contains("update") || a.ToLower().Contains("edit"));
            var deleteCount = allActions.Count(a =>
                a.Contains("حذف") || a.Contains("إلغاء") || a.Contains("الغاء") ||
                a.ToLower().Contains("delete") || a.ToLower().Contains("remove"));

            // ✅ Query 2: جيب الـ items مع كل البيانات
            var items = await query
                .OrderByDescending(x => x.Timestamp)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (items, totalCount, createCount, updateCount, deleteCount);
        }
    }
}