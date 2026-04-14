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

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllProjectsAsync();
            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            if (project == null) return null;
            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(ProjectRequestDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _unitOfWork.Projects.AddProjectAsync(project);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<bool> UpdateProjectAsync(int id, ProjectRequestDto projectDto)
        {
            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            if (project == null) return false;

            _mapper.Map(projectDto, project);
            _unitOfWork.Projects.UpdateProject(project);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            if (project == null) return false;

            project.IsDeleted = true;
            _unitOfWork.Projects.UpdateProject(project);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
