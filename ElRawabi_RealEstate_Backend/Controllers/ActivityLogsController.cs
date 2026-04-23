using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ActivityLogsController : ControllerBase
{
    private readonly IActivityLogService _logService;
    public ActivityLogsController(IActivityLogService logService) => _logService = logService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _logService.GetAllActivityLogsAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _logService.GetActivityLogByIdAsync(id));
}