using ElRawabi_RealEstate_Backend.Dtos.Auth;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ElRawabi_RealEstate_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login/staff")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> LoginStaff([FromBody] LoginRequestDto loginDto)
        {
            var result = await _authService.LoginUserAsync(loginDto);
            return result == null ? Unauthorized(new { message = "بيانات الدخول غير صحيحة" }) : Ok(result);
        }

        [HttpPost("login/buyer")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> LoginBuyer([FromBody] LoginRequestDto loginDto)
        {
            var result = await _authService.LoginBuyerAsync(loginDto);
            return result == null ? Unauthorized(new { message = "بيانات الدخول غير صحيحة" }) : Ok(result);
        }
    }
}