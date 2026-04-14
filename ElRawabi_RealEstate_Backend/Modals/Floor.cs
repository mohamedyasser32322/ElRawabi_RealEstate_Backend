using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public class Floor
    {
        public int Id { get; set; }

        [Required]
        public int FloorNumber { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalUnits { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int AvailableUnits { get; set; } = 0;

        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Building Building { get; set; }
        public int BuildingId { get; set; }
        public List<Unit> Units { get; set; } = new List<Unit>();
    }
}