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

    public ConstructionStagesController(IConstructionStageService stageService)
        => _stageService = stageService;

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    // ── GET /api/ConstructionStages
    // ── GET /api/ConstructionStages?buildingId=5   ← الفرونت بيستخدم ده
    // ── GET /api/ConstructionStages?BuildingId=5   ← fallback كابيتال
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? buildingId, [FromQuery] int? BuildingId)
    {
        var bid = buildingId ?? BuildingId;

        if (bid.HasValue && bid.Value > 0)
        {
            var byBuilding = await _stageService.GetByBuildingIdAsync(bid.Value);
            return Ok(byBuilding);
        }

        var all = await _stageService.GetAllConstructionStagesAsync();
        return Ok(all);
    }

    // ── GET /api/ConstructionStages/building/5   ← fallback endpoint
    [HttpGet("building/{buildingId:int}")]
    public async Task<IActionResult> GetByBuilding(int buildingId)
    {
        var stages = await _stageService.GetByBuildingIdAsync(buildingId);
        return Ok(stages);
    }

    // ── GET /api/ConstructionStages/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var stage = await _stageService.GetConstructionStageByIdAsync(id);
        return stage == null ? NotFound() : Ok(stage);
    }

    // ── POST /api/ConstructionStages
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] ConstructionStageRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _stageService.CreateConstructionStageAsync(dto, GetCurrentUserId());
        return Ok(result);
    }

    // ── PUT /api/ConstructionStages/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] ConstructionStageRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _stageService.UpdateConstructionStageAsync(id, dto, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }

    // ── DELETE /api/ConstructionStages/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _stageService.DeleteConstructionStageAsync(id, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }
}