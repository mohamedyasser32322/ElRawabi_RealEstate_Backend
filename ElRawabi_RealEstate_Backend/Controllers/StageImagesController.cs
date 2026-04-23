using ElRawabi_RealEstate_Backend.Dtos.Responses;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElRawabi_RealEstate_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StageImagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IActivityLogService _log;

        public StageImagesController(IUnitOfWork unitOfWork, IWebHostEnvironment env, IActivityLogService log)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _log = log;
        }

        private int? UserId()
        {
            var c = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(c, out var id) ? id : null;
        }

        private string GetWebRoot()
        {
            var webRoot = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");
                Directory.CreateDirectory(webRoot);
            }
            return webRoot;
        }

        [HttpGet]
        public async Task<IActionResult> GetByStage([FromQuery] int stageId)
        {
            var images = await _unitOfWork.StageImages.GetByStageIdAsync(stageId);
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var result = images.Select(i => new StageImageResponseDto
            {
                Id = i.Id,
                ConstructionStageId = i.ConstructionStageId,
                ImageUrl = $"{baseUrl}/{i.ImageUrl}",
                Caption = i.Caption,
                CreatedAt = i.CreatedAt
            });
            return Ok(result);
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Upload([FromForm] int stageId, [FromForm] string? caption, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "لم يتم إرسال ملف" });

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
                return BadRequest(new { message = "نوع الملف غير مدعوم. استخدم jpg/png/webp" });

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest(new { message = "حجم الملف يتجاوز 5MB" });

            if (!IsValidImage(file))
                return BadRequest(new { message = "محتوى الملف غير صحيح" });

            var folder = Path.Combine(GetWebRoot(), "uploads", "stages");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var relativePath = $"uploads/stages/{fileName}";
            var image = new StageImage
            {
                ConstructionStageId = stageId,
                ImageUrl = relativePath,
                Caption = caption,
            };

            await _unitOfWork.StageImages.AddAsync(image);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { image.ConstructionStageId, image.ImageUrl, image.Caption };

            await _log.LogActivityAsync(
                "إضافة", "صورة مرحلة", image.Id,
                $"رفع صورة للمرحلة {stageId}",
                UserId(),
                newValues: newSnapshot);

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return Ok(new StageImageResponseDto
            {
                Id = image.Id,
                ConstructionStageId = image.ConstructionStageId,
                ImageUrl = $"{baseUrl}/{relativePath}",
                Caption = image.Caption,
                CreatedAt = image.CreatedAt
            });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _unitOfWork.StageImages.GetByIdAsync(id);
            if (image == null) return NotFound();

            var oldSnapshot = new { image.ConstructionStageId, image.ImageUrl, image.Caption };

            var filePath = Path.Combine(GetWebRoot(), image.ImageUrl);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _unitOfWork.StageImages.Delete(image);
            await _unitOfWork.CompleteAsync();

            await _log.LogActivityAsync(
                "حذف", "صورة مرحلة", id,
                $"حذف صورة المرحلة {image.ConstructionStageId}",
                UserId(),
                oldValues: oldSnapshot);

            return Ok();
        }

        private static bool IsValidImage(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var header = new byte[4];
            stream.Read(header, 0, 4);

            // JPEG: FF D8 FF
            if (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF) return true;
            // PNG: 89 50 4E 47
            if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47) return true;
            // WEBP: 52 49 46 46
            if (header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46) return true;

            return false;
        }
    }
}