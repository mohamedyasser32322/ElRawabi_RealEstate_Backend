using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task SaveChangesAsync();
        Task<User?> GetUserByEmailAsync(string email);
    }
}
