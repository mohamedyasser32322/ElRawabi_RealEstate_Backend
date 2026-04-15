using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class BuyerRequestDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required, Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? NationalId { get; set; }
    }
}
