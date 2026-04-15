using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    public NotificationsController(INotificationService notificationService) => _notificationService = notificationService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _notificationService.GetAllNotificationsAsync());
    [HttpPatch("{id}/read")] public async Task<IActionResult> MarkAsRead(int id) => Ok(await _notificationService.MarkAsReadAsync(id));
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _notificationService.DeleteNotificationAsync(id));
}
