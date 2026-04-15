using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    public ProjectsController(IProjectService projectService) => _projectService = projectService;

    [HttpGet][AllowAnonymous] public async Task<IActionResult> GetAll() => Ok(await _projectService.GetAllProjectsAsync());
    [HttpGet("{id}")][AllowAnonymous] public async Task<IActionResult> GetById(int id) => Ok(await _projectService.GetProjectByIdAsync(id));
    [HttpPost][Authorize(Roles = "Admin")] public async Task<IActionResult> Create([FromBody] ProjectRequestDto dto) => Ok(await _projectService.CreateProjectAsync(dto));
    [HttpPut("{id}")][Authorize(Roles = "Admin")] public async Task<IActionResult> Update(int id, [FromBody] ProjectRequestDto dto) => Ok(await _projectService.UpdateProjectAsync(id, dto));
    [HttpDelete("{id}")][Authorize(Roles = "Admin")] public async Task<IActionResult> Delete(int id) => Ok(await _projectService.DeleteProjectAsync(id));
}
