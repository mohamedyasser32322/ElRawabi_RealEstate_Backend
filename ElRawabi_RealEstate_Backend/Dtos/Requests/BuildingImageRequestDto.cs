using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class BuildingImageRequestDto
    {
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsMain { get; set; }
        public int BuildingId { get; set; }
    }
}
