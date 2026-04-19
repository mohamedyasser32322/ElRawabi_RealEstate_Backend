using System.Collections.Generic;

namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class BuildingResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalUnits { get; set; }
        public int AvailableUnits { get; set; }
        public int ReservedUnits { get; set; }
        public int SoldUnits { get; set; }
        public decimal Progress { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<FloorResponseDto> Floors { get; set; } = new List<FloorResponseDto>();
    }
}
