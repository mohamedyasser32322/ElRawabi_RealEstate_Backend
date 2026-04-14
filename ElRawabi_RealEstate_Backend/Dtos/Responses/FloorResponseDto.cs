namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class FloorResponseDto
    {
        public int Id { get; set; }
        public int FloorNumber { get; set; }
        public int TotalUnits { get; set; }
        public int AvailableUnits { get; set; }
        public string? Description { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
