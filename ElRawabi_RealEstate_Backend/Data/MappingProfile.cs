using AutoMapper;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using System.Linq;

namespace ElRawabi_RealEstate_Backend.Mappings
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectResponseDto>()
                .ForMember(dest => dest.Buildings, opt => opt.MapFrom(src => src.Buildings.Where(b => !b.IsDeleted)))
                .ForMember(dest => dest.TotalUnits,
                    opt => opt.MapFrom(src =>
                        src.Buildings
                            .Where(b => !b.IsDeleted)
                            .SelectMany(b => b.Floors)
                            .Where(f => !f.IsDeleted)
                            .SelectMany(f => f.Units)
                            .Where(u => !u.IsDeleted)
                            .Count()))
                .ForMember(dest => dest.AvailableUnits,
                    opt => opt.MapFrom(src =>
                        src.Buildings
                            .Where(b => !b.IsDeleted)
                            .SelectMany(b => b.Floors)
                            .Where(f => !f.IsDeleted)
                            .SelectMany(f => f.Units)
                            .Where(u => !u.IsDeleted && u.Status == UnitStatus.Available)
                            .Count()))
                .ForMember(dest => dest.ReservedUnits,
                    opt => opt.MapFrom(src =>
                        src.Buildings
                            .Where(b => !b.IsDeleted)
                            .SelectMany(b => b.Floors)
                            .Where(f => !f.IsDeleted)
                            .SelectMany(f => f.Units)
                            .Where(u => !u.IsDeleted && u.Status == UnitStatus.Reserved)
                            .Count()))
                .ForMember(dest => dest.SoldUnits,
                    opt => opt.MapFrom(src =>
                        src.Buildings
                            .Where(b => !b.IsDeleted)
                            .SelectMany(b => b.Floors)
                            .Where(f => !f.IsDeleted)
                            .SelectMany(f => f.Units)
                            .Where(u => !u.IsDeleted && u.Status == UnitStatus.Sold)
                            .Count()));
            CreateMap<ProjectRequestDto, Project>();


            CreateMap<BuildingRequestDto, Building>();
            CreateMap<Building, BuildingResponseDto>()
                .ForMember(dest => dest.Floors, opt => opt.MapFrom(src => src.Floors.Where(f => !f.IsDeleted)))
                .ForMember(dest => dest.ProjectName,
                    opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : string.Empty))
                .ForMember(dest => dest.TotalUnits,
                    opt => opt.MapFrom(src =>
                        src.Floors
                            .Where(f => !f.IsDeleted)
                            .SelectMany(f => f.Units)
                            .Where(u => !u.IsDeleted)
                            .Count()))
                .ForMember(dest => dest.AvailableUnits,
                    opt => opt.MapFrom(src =>
                        src.Floors
                            .Where(f => !f.IsDeleted)
                            .SelectMany(f => f.Units)
                            .Where(u => !u.IsDeleted && u.Status == UnitStatus.Available)
                            .Count()))
                .ForMember(dest => dest.ReservedUnits,
                    opt => opt.MapFrom(src =>
                        src.Floors
                            .Where(f => !f.IsDeleted)
                            .SelectMany(f => f.Units)
                            .Where(u => !u.IsDeleted && u.Status == UnitStatus.Reserved)
                            .Count()))
                .ForMember(dest => dest.SoldUnits,
                    opt => opt.MapFrom(src =>
                        src.Floors
                            .Where(f => !f.IsDeleted)
                            .SelectMany(f => f.Units)
                            .Where(u => !u.IsDeleted && u.Status == UnitStatus.Sold)
                            .Count()));

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            CreateMap<UserCreateRequestDto, User>();
            CreateMap<UserUpdateRequestDto, User>();
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : string.Empty));

            CreateMap<BuyerRequestDto, Buyer>();
            CreateMap<Buyer, BuyerResponseDto>();

            CreateMap<FloorRequestDto, Floor>();
            CreateMap<Floor, FloorResponseDto>()
                .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Units.Where(u => !u.IsDeleted)))
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building != null ? src.Building.Name : string.Empty));
            
            CreateMap<UnitRequestDto, Unit>()
                 .ForMember(dest => dest.BuyerId, opt => opt.MapFrom(src => src.BuyerId));
            CreateMap<Unit, UnitResponseDto>()
                .ForMember(dest => dest.FloorNumber, opt => opt.MapFrom(src => src.Floor != null ? src.Floor.FloorNumber : 0));

            CreateMap<BookingRequestDto, Booking>();
            CreateMap<Booking, BookingResponseDto>()
                .ForMember(dest => dest.BuyerFullName, opt => opt.MapFrom(src => src.Buyer != null ? $"{src.Buyer.FirstName} {src.Buyer.LastName}" : string.Empty))
                .ForMember(dest => dest.UnitNumber, opt => opt.MapFrom(src => src.Unit != null ? src.Unit.UnitNumber : string.Empty));

            CreateMap<ConstructionStageRequestDto, ConstructionStage>();
            CreateMap<ConstructionStage, ConstructionStageResponseDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building != null ? src.Building.Name : string.Empty));

            CreateMap<BuildingImageRequestDto, BuildingImage>();
            CreateMap<BuildingImage, BuildingImageResponseDto>();

            CreateMap<NotificationRequestDto, Notification>();
            CreateMap<Notification, NotificationResponseDto>();

            CreateMap<ActivityLog, ActivityLogResponseDto>()
                .ForMember(dest => dest.EntityName,
                    opt => opt.MapFrom(src => src.Entity))
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User != null
                        ? (src.User.FirstName + " " + src.User.LastName).Trim()
                        : src.UserId.HasValue ? $"مستخدم #{src.UserId}" : null))
                .ForMember(dest => dest.UserRole,
                    opt => opt.MapFrom(src => src.User != null && src.User.Role != null
                        ? src.User.Role.RoleName
                        : null))
                .ForMember(dest => dest.OldValues,
                    opt => opt.MapFrom(src => src.OldValues))
                .ForMember(dest => dest.NewValues,
                    opt => opt.MapFrom(src => src.NewValues));
        }
    }
}
