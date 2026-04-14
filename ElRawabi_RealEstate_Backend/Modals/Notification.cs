namespace ElRawabi_RealEstate_Backend.Modals
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public User User { get; set; }
        public int? RecipientUserId { get; set; }
        public Buyer Buyer { get; set; }
        public int? RecipientBuyerId { get; set; }

    }
}
