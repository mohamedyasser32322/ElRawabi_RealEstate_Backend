using ElRawabi_RealEstate_Backend.Modals;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class ConstructionStageRequestDto
    {
        public int BuildingId { get; set; }

        [Required, MaxLength(255)]
        public string StageName { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StageStatus Status { get; set; } = StageStatus.Pending;

        public bool IsCompleted { get; set; } = false;

        public string? Notes { get; set; }

        public string? ReportData { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}