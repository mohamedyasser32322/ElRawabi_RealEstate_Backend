using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElRawabi_RealEstate_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        private int? CurrentUserId => int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _userService.GetAllUsersAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _userService.GetUserByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateRequestDto dto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(dto, CurrentUserId);
                return Ok(result);
            }
            catch (InvalidOperationException ex) when (ex.Message == "email_duplicate")
            {
                return Conflict(new { message = "البريد الإلكتروني مسجل مسبقاً" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateRequestDto dto)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, dto, CurrentUserId);
                return result ? Ok(result) : NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == "email_duplicate")
            {
                return Conflict(new { message = "البريد الإلكتروني مسجل مسبقاً" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _userService.DeleteUserAsync(id, CurrentUserId));
    }
}