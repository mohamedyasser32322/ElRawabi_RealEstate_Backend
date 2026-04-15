using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    public BookingsController(IBookingService bookingService) => _bookingService = bookingService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _bookingService.GetAllBookingsAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _bookingService.GetBookingByIdAsync(id));
    [HttpPost][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Create([FromBody] BookingRequestDto dto) => Ok(await _bookingService.CreateBookingAsync(dto));
    [HttpPut("{id}")][Authorize(Roles = "Admin,Manager")] public async Task<IActionResult> Update(int id, [FromBody] BookingRequestDto dto) => Ok(await _bookingService.UpdateBookingAsync(id, dto));
    [HttpDelete("{id}")][Authorize(Roles = "Admin")] public async Task<IActionResult> Delete(int id) => Ok(await _bookingService.DeleteBookingAsync(id));
}
