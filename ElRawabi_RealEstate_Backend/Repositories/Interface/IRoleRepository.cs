using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task AddRoleAsync(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        Task SaveChangesAsync();
        Task<Role?> GetRoleByNameAsync(string roleName);
    }
}
