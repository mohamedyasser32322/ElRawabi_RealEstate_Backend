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

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync()
        {
            var notifications = await _unitOfWork.Notifications.GetAllNotificationsAsync();
            return _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
        }

        public async Task<NotificationResponseDto?> GetNotificationByIdAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetNotificationByIdAsync(id);
            if (notification == null) return null;
            return _mapper.Map<NotificationResponseDto>(notification);
        }

        public async Task<NotificationResponseDto> CreateNotificationAsync(NotificationRequestDto notificationDto)
        {
            var notification = _mapper.Map<Notification>(notificationDto);
            await _unitOfWork.Notifications.AddNotificationAsync(notification);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<NotificationResponseDto>(notification);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetNotificationByIdAsync(id);
            if (notification == null) return false;

            notification.IsRead = true;
            _unitOfWork.Notifications.UpdateNotification(notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetNotificationByIdAsync(id);
            if (notification == null) return false;

            notification.IsDeleted = true;
            _unitOfWork.Notifications.UpdateNotification(notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
