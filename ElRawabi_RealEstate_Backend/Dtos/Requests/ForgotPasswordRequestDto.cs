using ElRawabi_RealEstate_Backend.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Dtos.Requests
{
    public class ForgotPasswordRequestDto
    {
        [Required, Gmail]
        public string Email { get; set; } = string.Empty;
    }
}
