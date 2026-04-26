using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Modals;
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
    public ConstructionStagesController(IConstructionStageService stageService)
        => _stageService = stageService;

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? buildingId, [FromQuery] int? BuildingId)
    {
        var bid = buildingId ?? BuildingId;
        if (bid.HasValue && bid.Value > 0)
            return Ok(await _stageService.GetByBuildingIdAsync(bid.Value));
        return Ok(await _stageService.GetAllConstructionStagesAsync());
    }

    [HttpGet("building/{buildingId:int}")]
    public async Task<IActionResult> GetByBuilding(int buildingId)
        => Ok(await _stageService.GetByBuildingIdAsync(buildingId));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var stage = await _stageService.GetConstructionStageByIdAsync(id);
        return stage == null ? NotFound() : Ok(stage);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SiteEngineer")]
    public async Task<IActionResult> Create([FromBody] ConstructionStageRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _stageService.CreateConstructionStageAsync(dto, GetCurrentUserId());
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,SiteEngineer")]
    public async Task<IActionResult> Update(int id, [FromBody] ConstructionStageRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _stageService.UpdateConstructionStageAsync(id, dto, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _stageService.DeleteConstructionStageAsync(id, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }
}