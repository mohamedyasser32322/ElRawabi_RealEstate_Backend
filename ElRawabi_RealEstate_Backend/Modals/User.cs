using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string HashPassword { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public List<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}