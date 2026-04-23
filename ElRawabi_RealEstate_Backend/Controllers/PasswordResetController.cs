using ElRawabi_RealEstate_Backend.Dtos.Requests;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class PasswordResetController : ControllerBase
{
    private readonly IPasswordResetService _service;

    public PasswordResetController(IPasswordResetService service)
    {
        _service = service;
    }

    [HttpPost("forgot")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
    {
        var result = await _service.SendResetTokenAsync(dto.Email);

        if (!result)
            return NotFound(new { message = "البريد الإلكتروني غير مسجل في النظام." });

        return Ok(new { message = "تم إرسال رابط الاستعادة على بريدك الإلكتروني." });
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
    {
        var result = await _service.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);
        if (!result)
            return BadRequest(new { message = "الرمز غير صحيح أو انتهت صلاحيته." });

        return Ok(new { message = "تم تغيير كلمة المرور بنجاح." });
    }
}