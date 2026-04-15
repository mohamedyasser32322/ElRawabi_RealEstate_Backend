using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync() => _mapper.Map<IEnumerable<NotificationResponseDto>>(await _unitOfWork.Notifications.GetAllNotificationsAsync());
        public async Task<NotificationResponseDto?> GetNotificationByIdAsync(int id) => _mapper.Map<NotificationResponseDto>(await _unitOfWork.Notifications.GetNotificationByIdAsync(id));

        public async Task<NotificationResponseDto> CreateNotificationAsync(NotificationRequestDto notificationDto)
        {
            var notification = _mapper.Map<Notification>(notificationDto);
            await _unitOfWork.Notifications.AddNotificationAsync(notification);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إرسال", "تنبيه", notification.Id, $"تم إرسال تنبيه جديد", null);
            return _mapper.Map<NotificationResponseDto>(notification);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetNotificationByIdAsync(id);
            if (notification == null) return false;
            notification.IsRead = true;
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetNotificationByIdAsync(id);
            if (notification == null) return false;
            notification.IsDeleted = true;
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
