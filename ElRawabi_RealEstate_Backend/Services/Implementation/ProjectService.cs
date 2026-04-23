using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync() =>
            _mapper.Map<IEnumerable<ProjectResponseDto>>(await _unitOfWork.Projects.GetAllProjectsAsync());

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            var p = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            return p == null ? null : _mapper.Map<ProjectResponseDto>(p);
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(ProjectRequestDto projectDto, int? currentUserId)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _unitOfWork.Projects.AddProjectAsync(project);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { project.Name, project.Location, project.TotalUnits };

            await _activityLogService.LogActivityAsync(
                "إضافة", "مشروع", project.Id,
                $"إنشاء مشروع جديد: {project.Name}",
                currentUserId,
                newValues: newSnapshot);

            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<bool> UpdateProjectAsync(int id, ProjectRequestDto projectDto, int? currentUserId)
        {
            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            if (project == null) return false;

            var oldSnapshot = new { project.Name, project.Location, project.TotalUnits };

            _mapper.Map(projectDto, project);
            _unitOfWork.Projects.UpdateProject(project);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { project.Name, project.Location, project.TotalUnits };

            await _activityLogService.LogActivityAsync(
                "تعديل", "مشروع", id,
                $"تعديل بيانات مشروع {project.Name}",
                currentUserId,
                oldValues: oldSnapshot,
                newValues: newSnapshot);

            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id, int? currentUserId)
        {
            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            if (project == null) return false;

            var oldSnapshot = new
            {
                project.Name,
                project.Location,
                project.TotalUnits,
                BuildingCount = project.Buildings?.Count ?? 0
            };

            foreach (var building in project.Buildings)
            {
                foreach (var floor in building.Floors)
                {
                    foreach (var unit in floor.Units)
                    {
                        if (unit.Booking != null)
                        {
                            unit.Booking.IsDeleted = true;
                            _unitOfWork.Bookings.UpdateBooking(unit.Booking);
                        }
                        unit.IsDeleted = true;
                        _unitOfWork.Units.UpdateUnit(unit);
                    }
                    floor.IsDeleted = true;
                    _unitOfWork.Floors.UpdateFloor(floor);
                }
                building.IsDeleted = true;
                _unitOfWork.Buildings.UpdateBuilding(building);
            }

            project.IsDeleted = true;
            _unitOfWork.Projects.UpdateProject(project);
            await _unitOfWork.CompleteAsync();

            await _activityLogService.LogActivityAsync(
                "حذف", "مشروع", id,
                $"حذف مشروع {project.Name} مع جميع محتوياته",
                currentUserId,
                oldValues: oldSnapshot);

            return true;
        }
    }
}