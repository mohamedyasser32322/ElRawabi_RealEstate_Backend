using ElRawabi_RealEstate_Backend.Data;
using ElRawabi_RealEstate_Backend.Repositories.Interface;

namespace ElRawabi_RealEstate_Backend.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ElRawabiRealEstateDbContext _context;

        public IProjectRepository Projects { get; private set; }
        public IUserRepository Users { get; private set; }
        public IBuyerRepository Buyers { get; private set; }
        public IBuildingRepository Buildings { get; private set; }
        public IFloorRepository Floors { get; private set; }
        public IUnitRepository Units { get; private set; }
        public IBookingRepository Bookings { get; private set; }
        public IConstructionStageRepository ConstructionStages { get; private set; }
        public IActivityLogRepository ActivityLogs { get; private set; }
        public IBuildingImageRepository BuildingImages { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IStageImageRepository StageImages { get; private set; }

        public UnitOfWork(ElRawabiRealEstateDbContext context)
        {
            _context = context;
            Projects = new ProjectRepository(_context);
            Users = new UserRepository(_context);
            Buyers = new BuyerRepository(_context);
            Buildings = new BuildingRepository(_context);
            Floors = new FloorRepository(_context);
            Units = new UnitRepository(_context);
            Bookings = new BookingRepository(_context);
            ConstructionStages = new ConstructionStageRepository(_context);
            ActivityLogs = new ActivityLogRepository(_context);
            BuildingImages = new BuildingImageRepository(_context);
            Notifications = new NotificationRepository(_context);
            Roles = new RoleRepository(_context);
            StageImages = new StageImageRepository(_context);
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}