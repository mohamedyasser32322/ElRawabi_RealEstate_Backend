using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class UserUpdateRequestDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
    }
}
