using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class ProjectRequestDto
    {
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
