using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class BuildingRequestDto
    {
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ProjectId { get; set; }
    }
}
