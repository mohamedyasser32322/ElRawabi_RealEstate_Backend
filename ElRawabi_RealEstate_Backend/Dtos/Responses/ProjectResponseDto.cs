namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalBuildings { get; set; }
        public int TotalUnits { get; set; }
        public int AvailableUnits { get; set; }
        public int ReservedUnits { get; set; }
        public int SoldUnits { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
