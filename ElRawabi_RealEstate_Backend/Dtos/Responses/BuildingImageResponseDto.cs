namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class BuildingImageResponseDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsMain { get; set; }
        public int BuildingId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
