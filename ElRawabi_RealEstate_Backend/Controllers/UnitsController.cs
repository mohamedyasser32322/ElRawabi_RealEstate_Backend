using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UnitsController : ControllerBase
{
    private readonly IUnitService _unitService;
    public UnitsController(IUnitService unitService) => _unitService = unitService;

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _unitService.GetAllUnitsAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var unit = await _unitService.GetUnitByIdAsync(id);
        return unit == null ? NotFound() : Ok(unit);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] UnitRequestDto dto) =>
        Ok(await _unitService.CreateUnitAsync(dto, GetCurrentUserId()));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] UnitRequestDto dto)
    {
        var result = await _unitService.UpdateUnitAsync(id, dto, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _unitService.DeleteUnitAsync(id, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }
}