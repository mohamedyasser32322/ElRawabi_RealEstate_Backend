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
            CreateMap<ProjectRequestDto, Project>();
            CreateMap<Project, ProjectResponseDto>()

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


            CreateMap<BuildingRequestDto, Building>();
            CreateMap<Building, BuildingResponseDto>()
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

            // Role
            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            // User
            CreateMap<UserCreateRequestDto, User>();
            CreateMap<UserUpdateRequestDto, User>();
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : string.Empty));

            // Buyer
            CreateMap<BuyerRequestDto, Buyer>();
            CreateMap<Buyer, BuyerResponseDto>();

            // Floor
            CreateMap<FloorRequestDto, Floor>();
            CreateMap<Floor, FloorResponseDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building != null ? src.Building.Name : string.Empty));

            // Unit
            CreateMap<UnitRequestDto, Unit>();
            CreateMap<Unit, UnitResponseDto>()
                .ForMember(dest => dest.FloorNumber, opt => opt.MapFrom(src => src.Floor != null ? src.Floor.FloorNumber : 0));

            // Booking
            CreateMap<BookingRequestDto, Booking>();
            CreateMap<Booking, BookingResponseDto>()
                .ForMember(dest => dest.BuyerFullName, opt => opt.MapFrom(src => src.Buyer != null ? $"{src.Buyer.FirstName} {src.Buyer.LastName}" : string.Empty))
                .ForMember(dest => dest.UnitNumber, opt => opt.MapFrom(src => src.Unit != null ? src.Unit.UnitNumber : string.Empty));

            // ConstructionStage
            CreateMap<ConstructionStageRequestDto, ConstructionStage>();
            CreateMap<ConstructionStage, ConstructionStageResponseDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building != null ? src.Building.Name : string.Empty));

            // BuildingImage
            CreateMap<BuildingImageRequestDto, BuildingImage>();
            CreateMap<BuildingImage, BuildingImageResponseDto>();

            // Notification
            CreateMap<NotificationRequestDto, Notification>();
            CreateMap<Notification, NotificationResponseDto>();

            // ActivityLog
            CreateMap<ActivityLog, ActivityLogResponseDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : string.Empty));
        }
    }
}
