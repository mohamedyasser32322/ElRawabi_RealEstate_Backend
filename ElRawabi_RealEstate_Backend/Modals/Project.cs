using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public class Project
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Location { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalBuildings { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int TotalUnits { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int AvailableUnits { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<Building> Buildings { get; set; } = new List<Building>();
    }
}
