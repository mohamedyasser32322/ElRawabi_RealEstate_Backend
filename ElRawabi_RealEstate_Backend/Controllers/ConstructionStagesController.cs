using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ConstructionStagesController : ControllerBase
{
    private readonly IConstructionStageService _stageService;
    public ConstructionStagesController(IConstructionStageService stageService) => _stageService = stageService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _stageService.GetAllConstructionStagesAsync());
    [HttpPost][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Create([FromBody] ConstructionStageRequestDto dto) => Ok(await _stageService.CreateConstructionStageAsync(dto));
    [HttpDelete("{id}")][Authorize(Roles = "Admin")] public async Task<IActionResult> Delete(int id) => Ok(await _stageService.DeleteConstructionStageAsync(id));
}
