using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Helpers;
using ElRawabi_RealEstate_Backend.Modals;
using Microsoft.EntityFrameworkCore;

namespace ElRawabi_RealEstate_Backend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ElRawabiRealEstateDbContext context)
        {
            if (!await context.Users.AnyAsync(u => u.Email == "admin@elrawabi.com"))
            {
                var adminRole = await context.Roles.FirstAsync(r => r.RoleName == "Admin");

                var adminUser = new User
                {
                    FirstName = "System",
                    LastName = "Admin",
                    Email = "admin@elrawabi.com",
                    HashPassword = PasswordHelper.HashPassword("Admin@123"),
                    RoleId = adminRole.Id,
                    IsActive = true
                };
                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}