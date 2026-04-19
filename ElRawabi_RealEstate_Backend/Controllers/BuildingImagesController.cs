using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BuildingImagesController : ControllerBase
{
    private readonly IBuildingImageService _imageService;
    public BuildingImagesController(IBuildingImageService imageService) => _imageService = imageService;

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _imageService.GetAllBuildingImagesAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var image = await _imageService.GetBuildingImageByIdAsync(id);
        return image == null ? NotFound() : Ok(image);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] BuildingImageRequestDto dto) =>
        Ok(await _imageService.CreateBuildingImageAsync(dto, GetCurrentUserId()));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] BuildingImageRequestDto dto)
    {
        var result = await _imageService.UpdateBuildingImageAsync(id, dto, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _imageService.DeleteBuildingImageAsync(id, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }
}