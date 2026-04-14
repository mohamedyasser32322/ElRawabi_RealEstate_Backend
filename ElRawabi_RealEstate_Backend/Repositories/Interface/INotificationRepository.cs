using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification?> GetNotificationByIdAsync(int id);
        Task AddNotificationAsync(Notification notification);
        void UpdateNotification(Notification notification);
        void DeleteNotification(Notification notification);
        Task SaveChangesAsync();
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
    }
}
