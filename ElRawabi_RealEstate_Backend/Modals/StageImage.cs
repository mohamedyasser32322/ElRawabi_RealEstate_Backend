using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
        public class StageImage
        {
            public int Id { get; set; }

            public int ConstructionStageId { get; set; }
            public ConstructionStage ConstructionStage { get; set; } = null!;

            [Required, MaxLength(500)]
            public string ImageUrl { get; set; } = string.Empty;

            [MaxLength(300)]
            public string? Caption { get; set; }

            public bool IsDeleted { get; set; } = false;
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }
