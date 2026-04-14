namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }
        IUserRepository Users { get; }
        IBuyerRepository Buyers { get; }
        IBuildingRepository Buildings { get; }
        IFloorRepository Floors { get; }
        IUnitRepository Units { get; }
        IBookingRepository Bookings { get; }
        IConstructionStageRepository ConstructionStages { get; }
        IActivityLogRepository ActivityLogs { get; }
        IBuildingImageRepository BuildingImages { get; }
        INotificationRepository Notifications { get; }
        IRoleRepository Roles { get; }

        Task<int> CompleteAsync();
    }
}
