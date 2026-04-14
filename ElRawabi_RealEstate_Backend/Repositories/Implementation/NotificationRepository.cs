using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ElRawabiRealEstateDbContext _context;
        private readonly DbSet<Notification> _dbSet;

        public NotificationRepository(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Notification>();
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync() => await _dbSet.ToListAsync();
        public async Task<Notification?> GetNotificationByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddNotificationAsync(Notification notification) => await _dbSet.AddAsync(notification);
        public void UpdateNotification(Notification notification) => _dbSet.Update(notification);
        public void DeleteNotification(Notification notification) => _dbSet.Remove(notification);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(n => n.RecipientUserId == userId).ToListAsync();
        }
    }
}


