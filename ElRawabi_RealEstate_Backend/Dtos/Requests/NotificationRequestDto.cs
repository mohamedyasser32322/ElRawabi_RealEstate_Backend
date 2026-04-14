using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class NotificationRequestDto
    {
        [Required]
        public string Message { get; set; } = string.Empty;
        public int? RecipientUserId { get; set; }
        public int? RecipientBuyerId { get; set; }
    }
}
