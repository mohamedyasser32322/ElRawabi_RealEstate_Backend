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

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllProjectsAsync();
            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            return project == null ? null : _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(ProjectRequestDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _unitOfWork.Projects.AddProjectAsync(project);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "مشروع", project.Id, $"تم إنشاء مشروع جديد: {project.Name}", null);
            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<bool> UpdateProjectAsync(int id, ProjectRequestDto projectDto)
        {
            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            if (project == null) return false;
            _mapper.Map(projectDto, project);
            _unitOfWork.Projects.UpdateProject(project);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "مشروع", id, $"تم تعديل بيانات مشروع {project.Name}", null);
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            if (project == null) return false;
            project.IsDeleted = true;
            _unitOfWork.Projects.UpdateProject(project);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "مشروع", id, $"تم حذف مشروع {project.Name}", null);
            return true;
        }
    }
}
