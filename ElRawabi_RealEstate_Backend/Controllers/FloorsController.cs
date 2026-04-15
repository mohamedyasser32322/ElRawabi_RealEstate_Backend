using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class FloorsController : ControllerBase
{
    private readonly IFloorService _floorService;
    public FloorsController(IFloorService floorService) => _floorService = floorService;

    [HttpGet][AllowAnonymous] public async Task<IActionResult> GetAll() => Ok(await _floorService.GetAllFloorsAsync());
    [HttpGet("{id}")][AllowAnonymous] public async Task<IActionResult> GetById(int id) => Ok(await _floorService.GetFloorByIdAsync(id));
    [HttpPost][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Create([FromBody] FloorRequestDto dto) => Ok(await _floorService.CreateFloorAsync(dto));
    [HttpPut("{id}")][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Update(int id, [FromBody] FloorRequestDto dto) => Ok(await _floorService.UpdateFloorAsync(id, dto));
    [HttpDelete("{id}")][Authorize(Roles = "Admin")] public async Task<IActionResult> Delete(int id) => Ok(await _floorService.DeleteFloorAsync(id));
}
