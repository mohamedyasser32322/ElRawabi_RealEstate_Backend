using System.ComponentModel.DataAnnotations;
using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class ConstructionStageRequestDto
    {
        [Required, MaxLength(255)]
        public string StageName { get; set; } = string.Empty;
        public StageStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int BuildingId { get; set; }
    }
}
