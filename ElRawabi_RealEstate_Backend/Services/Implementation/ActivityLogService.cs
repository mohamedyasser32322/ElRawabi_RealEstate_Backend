using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityLogResponseDto>> GetAllActivityLogsAsync()
        {
            var logs = await _unitOfWork.ActivityLogs.GetAllActivityLogsAsync();
            return _mapper.Map<IEnumerable<ActivityLogResponseDto>>(logs);
        }

        public async Task<ActivityLogResponseDto?> GetActivityLogByIdAsync(int id)
        {
            var log = await _unitOfWork.ActivityLogs.GetActivityLogByIdAsync(id);
            if (log == null) return null;
            return _mapper.Map<ActivityLogResponseDto>(log);
        }

        public async Task LogActivityAsync(string action, string entity, int entityId, string? details, int? userId)
        {
            var log = new ActivityLog
            {
                Action = action,
                Entity = entity,
                EntityId = entityId,
                Details = details,
                UserId = userId,
                Timestamp = DateTime.UtcNow
            };
            await _unitOfWork.ActivityLogs.AddActivityLogAsync(log);
            await _unitOfWork.CompleteAsync();
        }
    }
}
