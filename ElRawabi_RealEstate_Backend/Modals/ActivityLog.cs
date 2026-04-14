namespace ElRawabi_RealEstate_Backend.Modals
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Details { get; set; }
        public bool IsDeleted { get; set; } = false;
        public User User { get; set; }
        public int? UserId { get; set; }
    }
}
