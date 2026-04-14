namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class BuildingResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalUnits { get; set; }
        public int AvailableUnits { get; set; }
        public decimal Progress { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
