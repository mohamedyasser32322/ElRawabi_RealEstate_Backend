using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public enum UnitStatus { Available = 1, Reserved = 2, Sold = 3, Closed = 4 }
    public enum UnitType { GroundFloor = 1, TypicalFloor = 2, Roof = 3 }

    public class Unit
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string UnitNumber { get; set; } = string.Empty;

        [Required]
        public UnitType Type { get; set; }

        [Required]
        public UnitStatus Status { get; set; }

        [Range(0, 10000)]
        public decimal Area { get; set; }

        [Range(0, 50)]
        public int Rooms { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int? BuyerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Floor Floor { get; set; }
        public int FloorId { get; set; }
        public Booking Booking { get; set; }
        public int? BookingId { get; set; }
    }
}