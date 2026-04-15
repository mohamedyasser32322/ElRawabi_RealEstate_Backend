using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class BuyersController : ControllerBase
{
    private readonly IBuyerService _buyerService;
    public BuyersController(IBuyerService buyerService) => _buyerService = buyerService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _buyerService.GetAllBuyersAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _buyerService.GetBuyerByIdAsync(id));
    [HttpPost] public async Task<IActionResult> Create([FromBody] BuyerRequestDto dto) => Ok(await _buyerService.CreateBuyerAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, [FromBody] BuyerRequestDto dto) => Ok(await _buyerService.UpdateBuyerAsync(id, dto));
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _buyerService.DeleteBuyerAsync(id));
}
