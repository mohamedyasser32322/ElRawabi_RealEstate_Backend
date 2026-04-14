using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public class Buyer
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


        [Required, Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Address { get; set; }

        [MaxLength(50)]
        public string? NationalId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}