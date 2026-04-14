using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public enum StageStatus { NotStarted = 1, InProgress = 2, Completed = 3 }

    public class ConstructionStage
    {
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string StageName { get; set; } = string.Empty;

        public StageStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Building Building { get; set; }
        public int BuildingId { get; set; }

    }
}