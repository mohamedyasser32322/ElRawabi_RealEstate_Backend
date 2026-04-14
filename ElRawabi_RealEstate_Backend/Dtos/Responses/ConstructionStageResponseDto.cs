using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class ConstructionStageResponseDto
    {
        public int Id { get; set; }
        public string StageName { get; set; } = string.Empty;
        public StageStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
