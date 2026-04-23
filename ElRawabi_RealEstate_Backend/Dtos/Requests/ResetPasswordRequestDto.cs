using ElRawabi_RealEstate_Backend.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Dtos.Requests
{
    public class ResetPasswordRequestDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, Gmail]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
