using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,BookingManager")]
public class BuyersController : ControllerBase
{
    private readonly IBuyerService _buyerService;
    public BuyersController(IBuyerService buyerService) => _buyerService = buyerService;

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _buyerService.GetAllBuyersAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var buyer = await _buyerService.GetBuyerByIdAsync(id);
        return buyer == null ? NotFound() : Ok(buyer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BuyerRequestDto dto) =>
        Ok(await _buyerService.CreateBuyerAsync(dto, GetCurrentUserId()));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BuyerRequestDto dto)
    {
        var result = await _buyerService.UpdateBuyerAsync(id, dto, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _buyerService.DeleteBuyerAsync(id, GetCurrentUserId());
        return result ? Ok() : NotFound();
    }
}