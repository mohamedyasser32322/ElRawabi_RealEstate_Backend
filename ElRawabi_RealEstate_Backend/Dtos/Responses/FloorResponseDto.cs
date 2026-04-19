using System.Collections.Generic;

namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class FloorResponseDto
    {
        public int Id { get; set; }
        public int FloorNumber { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public ICollection<UnitResponseDto> Units { get; set; } = new List<UnitResponseDto>();
    }
}
