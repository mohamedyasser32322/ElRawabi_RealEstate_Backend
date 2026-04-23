using ElRawabi_RealEstate_Backend.Dtos.Requests;
using ElRawabi_RealEstate_Backend.Dtos.Responses;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IActivityLogService
    {
        Task<IEnumerable<ActivityLogResponseDto>> GetAllActivityLogsAsync();
        Task<ActivityLogResponseDto?> GetActivityLogByIdAsync(int id);
        Task LogActivityAsync(string action, string entity, int entityId, string? details, int? userId, object? oldValues = null, object? newValues = null);
        Task<PagedLogResponseDto> GetFilteredActivityLogsAsync(ActivityLogParamsDto filter);
    }
}