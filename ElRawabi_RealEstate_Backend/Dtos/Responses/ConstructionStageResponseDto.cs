using ElRawabi_RealEstate_Backend.Dtos.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using System.Text.Json.Serialization;

namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class ConstructionStageResponseDto
    {
        public int Id { get; set; }

        public int BuildingId { get; set; }
        public string BuildingName { get; set; } = string.Empty;

        public string StageName { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StageStatus Status { get; set; }

        public bool IsCompleted { get; set; }

        public string? Notes { get; set; }

        public string? ReportData { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<StageImageResponseDto> Images { get; set; } = new();
    }
}