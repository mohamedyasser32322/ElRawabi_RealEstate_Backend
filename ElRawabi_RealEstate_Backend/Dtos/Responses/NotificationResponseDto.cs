namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public int? RecipientUserId { get; set; }
        public int? RecipientBuyerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
