using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _buildingService;
    public BuildingsController(IBuildingService buildingService) => _buildingService = buildingService;

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll() => Ok(await _buildingService.GetAllBuildingsAsync());

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var building = await _buildingService.GetBuildingByIdAsync(id);
        return building == null ? NotFound() : Ok(building);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] BuildingRequestDto dto) =>
        Ok(await _buildingService.CreateBuildingAsync(dto, GetCurrentUserId()));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] BuildingRequestDto dto)
    {
        var result = await _buildingService.UpdateBuildingAsync(id, dto, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _buildingService.DeleteBuildingAsync(id, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }
}