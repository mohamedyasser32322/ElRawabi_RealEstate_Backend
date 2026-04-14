using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class RoleRequestDto
    {
        [Required, MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
