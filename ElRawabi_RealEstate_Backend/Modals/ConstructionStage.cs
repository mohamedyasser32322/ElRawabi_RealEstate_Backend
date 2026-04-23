using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public enum StageStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }

    public class ConstructionStage
    {
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string StageName { get; set; } = string.Empty;

        public StageStatus Status { get; set; } = StageStatus.Pending;

        public bool IsCompleted { get; set; } = false;

        public string? Notes { get; set; }

        public string? ReportData { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public Building Building { get; set; } = null!;
        public int BuildingId { get; set; }
        public ICollection<StageImage> Images { get; set; } = new List<StageImage>();
    }
}

