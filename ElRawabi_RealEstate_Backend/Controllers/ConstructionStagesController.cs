using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ConstructionStagesController : ControllerBase
{
    private readonly IConstructionStageService _stageService;
    public ConstructionStagesController(IConstructionStageService stageService) => _stageService = stageService;

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _stageService.GetAllConstructionStagesAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var stage = await _stageService.GetConstructionStageByIdAsync(id);
        return stage == null ? NotFound() : Ok(stage);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] ConstructionStageRequestDto dto) =>
        Ok(await _stageService.CreateConstructionStageAsync(dto, GetCurrentUserId()));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] ConstructionStageRequestDto dto)
    {
        var result = await _stageService.UpdateConstructionStageAsync(id, dto, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _stageService.DeleteConstructionStageAsync(id, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }
}