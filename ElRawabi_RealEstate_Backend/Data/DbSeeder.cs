using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Helpers;
using ElRawabi_RealEstate_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace ElRawabi_RealEstate_Backend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ElRawabiRealEstateDbContext context)
        {
            if (await context.Roles.AnyAsync()) return;

            var adminRole = new Role { RoleName = "Admin" };
            var bookingManagerRole = new Role { RoleName = "BookingManager" };
            var siteEngineerRole = new Role { RoleName = "SiteEngineer" };
            await context.Roles.AddRangeAsync(adminRole, bookingManagerRole, siteEngineerRole);
            await context.SaveChangesAsync();

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

            var buyer = new Buyer
            {
                FirstName = "Test",
                LastName = "Buyer",
                Email = "buyer@test.com",
                HashPassword = PasswordHelper.HashPassword("Buyer@123"),
                PhoneNumber = "0500000000",
                NationalId = "1000000000"
            };
            await context.Buyers.AddAsync(buyer);
            await context.SaveChangesAsync();

            var project = new Project { Name = "Project_Alpha", Location = "Riyadh", Description = "Residential Complex", TotalUnits = 10, AvailableUnits = 10 };
            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            var building = new Building { Name = "Building_A1", ProjectId = project.Id, TotalUnits = 5, AvailableUnits = 5 };
            await context.Buildings.AddAsync(building);
            await context.SaveChangesAsync();

            await context.Stages.AddAsync(new ConstructionStage
            {
                BuildingId = building.Id,
                StageName = "Foundation",
            });

            var floor = new Floor { FloorNumber = 1, BuildingId = building.Id };
            await context.Floors.AddAsync(floor);
            await context.SaveChangesAsync();

            for (int i = 1; i <= 5; i++)
            {
                await context.Units.AddAsync(new Unit
                {
                    UnitNumber = $"U-10{i}",
                    FloorId = floor.Id,
                    Price = 800000,
                    Area = 150,
                    Rooms = 4,
                    Type = UnitType.TypicalFloor,
                    Status = UnitStatus.Available,
                    CreatedAt = DateTime.UtcNow
                });
            }
            await context.SaveChangesAsync();

            var unitToBook = await context.Units.FirstAsync();
            var booking = new Booking
            {
                UnitId = unitToBook.Id,
                BuyerId = buyer.Id,
                BookingDate = DateTime.UtcNow,
                Status = BookingStatus.Confirmed
            };
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();

            unitToBook.Status = UnitStatus.Reserved;
            unitToBook.BuyerId = buyer.Id;
            unitToBook.BookingId = booking.Id;

            await context.Notifications.AddAsync(new Notification
            {
                Message = "Seeded Successfully",
                IsRead = false
            });

            await context.SaveChangesAsync();
        }
    }
}
