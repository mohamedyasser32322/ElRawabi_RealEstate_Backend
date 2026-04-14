using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public class Building
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalUnits { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int AvailableUnits { get; set; } = 0;

        [Range(0, 100)]
        public decimal Progress { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public List<Floor> Floors { get; set; } = new List<Floor>();
        public List<ConstructionStage> Stages { get; set; } = new List<ConstructionStage>();
        public List<BuildingImage> Images { get; set; } = new List<BuildingImage>();
    }
}