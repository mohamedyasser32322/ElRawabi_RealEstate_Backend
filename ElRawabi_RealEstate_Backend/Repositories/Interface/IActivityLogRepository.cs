using ElRawabi_RealEstate_Backend.Dtos.Requests;
using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IActivityLogRepository
    {
        Task<IEnumerable<ActivityLog>> GetAllActivityLogsAsync();
        Task<ActivityLog?> GetActivityLogByIdAsync(int id);
        Task AddActivityLogAsync(ActivityLog activityLog);
        void UpdateActivityLog(ActivityLog activityLog);
        void DeleteActivityLog(ActivityLog activityLog);
        Task SaveChangesAsync();
        Task<IEnumerable<ActivityLog>> GetActivityLogsByUserIdAsync(int userId);
        Task<(IEnumerable<ActivityLog> Items, int TotalCount, int CreateCount, int UpdateCount, int DeleteCount)> GetFilteredActivityLogsAsync(ActivityLogParamsDto filter);
    }
}
