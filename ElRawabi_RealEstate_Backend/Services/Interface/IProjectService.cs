using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync();
        Task<ProjectResponseDto?> GetProjectByIdAsync(int id);
        Task<ProjectResponseDto> CreateProjectAsync(ProjectRequestDto projectDto ,int? currentUserId);
        Task<bool> UpdateProjectAsync(int id, ProjectRequestDto projectDto, int? currentUserId);
        Task<bool> DeleteProjectAsync(int id, int? currentUserId);
    }
}
