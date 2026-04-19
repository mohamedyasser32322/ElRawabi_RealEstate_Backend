using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync();
        Task<NotificationResponseDto?> GetNotificationByIdAsync(int id);
        Task<NotificationResponseDto> CreateNotificationAsync(NotificationRequestDto notificationDto, int? currentUserId);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteNotificationAsync(int id);
    }
}
