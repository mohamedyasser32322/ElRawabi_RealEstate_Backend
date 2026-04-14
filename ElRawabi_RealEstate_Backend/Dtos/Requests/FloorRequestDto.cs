using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class FloorRequestDto
    {
        [Required]
        public int FloorNumber { get; set; }
        public string? Description { get; set; }
        public int BuildingId { get; set; }
    }
}
