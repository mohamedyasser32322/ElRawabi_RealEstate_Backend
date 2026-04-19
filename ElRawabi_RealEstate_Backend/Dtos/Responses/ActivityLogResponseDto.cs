namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class ActivityLogResponseDto
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;
        public int EntityId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Details { get; set; }
        public int? UserId { get; set; }

        public string? UserName { get; set; }

        public string? UserRole { get; set; }
    }
}