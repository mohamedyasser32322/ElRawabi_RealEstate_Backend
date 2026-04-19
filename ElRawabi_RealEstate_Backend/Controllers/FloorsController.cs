using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FloorsController : ControllerBase
{
    private readonly IFloorService _floorService;
    public FloorsController(IFloorService floorService) => _floorService = floorService;

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _floorService.GetAllFloorsAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var floor = await _floorService.GetFloorByIdAsync(id);
        return floor == null ? NotFound() : Ok(floor);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] FloorRequestDto dto) =>
        Ok(await _floorService.CreateFloorAsync(dto, GetCurrentUserId()));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] FloorRequestDto dto)
    {
        var result = await _floorService.UpdateFloorAsync(id, dto, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _floorService.DeleteFloorAsync(id, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }
}