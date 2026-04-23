namespace ElRawabi_RealEstate_Backend.Dtos.Responses
{
    public class StageImageResponseDto
    {
        public int Id { get; set; }
        public int ConstructionStageId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
