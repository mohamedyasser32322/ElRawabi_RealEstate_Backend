using ElRawabi_RealEstate_Backend.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class UserCreateRequestDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, Gmail, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }
    }
}
