using System.ComponentModel.DataAnnotations;
using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class UnitRequestDto
    {
        [Required, MaxLength(50)]
        public string UnitNumber { get; set; } = string.Empty;
        [Required]
        public UnitType Type { get; set; }
        [Required]
        public UnitStatus Status { get; set; }
        [Required]
        public UnitFacing Facing { get; set; }
        [Range(0, 10000)]
        public decimal Area { get; set; }
        [Range(0, 50)]
        public int Rooms { get; set; }
        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public int FloorId { get; set; }
        public int? BuyerId { get; set; }
    }
}
