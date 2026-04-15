using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UnitsController : ControllerBase
{
    private readonly IUnitService _unitService;
    public UnitsController(IUnitService unitService) => _unitService = unitService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _unitService.GetAllUnitsAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _unitService.GetUnitByIdAsync(id));
    [HttpPost][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Create([FromBody] UnitRequestDto dto) => Ok(await _unitService.CreateUnitAsync(dto));
    [HttpPut("{id}")][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Update(int id, [FromBody] UnitRequestDto dto) => Ok(await _unitService.UpdateUnitAsync(id, dto));
    [HttpDelete("{id}")][Authorize(Roles = "Admin")] public async Task<IActionResult> Delete(int id) => Ok(await _unitService.DeleteUnitAsync(id));
}
