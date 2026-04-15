using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _buildingService;
    public BuildingsController(IBuildingService buildingService) => _buildingService = buildingService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _buildingService.GetAllBuildingsAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _buildingService.GetBuildingByIdAsync(id));
    [HttpPost][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Create([FromBody] BuildingRequestDto dto) => Ok(await _buildingService.CreateBuildingAsync(dto));
    [HttpPut("{id}")][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Update(int id, [FromBody] BuildingRequestDto dto) => Ok(await _buildingService.UpdateBuildingAsync(id, dto));
    [HttpDelete("{id}")][Authorize(Roles = "Admin")] public async Task<IActionResult> Delete(int id) => Ok(await _buildingService.DeleteBuildingAsync(id));
}
