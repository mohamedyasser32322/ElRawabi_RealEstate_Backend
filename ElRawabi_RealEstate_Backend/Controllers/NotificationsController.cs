using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    public NotificationsController(INotificationService notificationService) => _notificationService = notificationService;
    [HttpPatch("{id}/read")] public async Task<IActionResult> MarkAsRead(int id) => Ok(await _notificationService.MarkAsReadAsync(id));
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _notificationService.DeleteNotificationAsync(id));
    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string? GetCurrentRole()
        => User.FindFirst(ClaimTypes.Role)?.Value;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (GetCurrentRole() == "Admin")
            return Ok(await _notificationService.GetAllNotificationsAsync());

        var userId = GetCurrentUserId();
        var notifications = await _notificationService.GetAllNotificationsAsync();
        var filtered = notifications.Where(n =>
            n.RecipientUserId == userId || n.RecipientBuyerId == userId);
        return Ok(filtered);
    }
}
