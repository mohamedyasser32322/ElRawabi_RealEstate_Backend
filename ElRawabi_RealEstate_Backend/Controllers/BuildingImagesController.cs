using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BuildingImagesController : ControllerBase
{
    private readonly IBuildingImageService _imageService;
    public BuildingImagesController(IBuildingImageService imageService) => _imageService = imageService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _imageService.GetAllBuildingImagesAsync());
    [HttpPost][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Create([FromBody] BuildingImageRequestDto dto) => Ok(await _imageService.CreateBuildingImageAsync(dto));
    [HttpDelete("{id}")][Authorize(Roles = "Admin")] public async Task<IActionResult> Delete(int id) => Ok(await _imageService.DeleteBuildingImageAsync(id));
}
