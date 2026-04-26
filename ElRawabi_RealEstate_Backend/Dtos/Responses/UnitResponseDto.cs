using ElRawabi_RealEstate_Backend.Modals;
using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class UnitResponseDto
    {
        public int Id { get; set; }
        public string UnitNumber { get; set; } = string.Empty;
        public UnitType Type { get; set; }
        public UnitStatus Status { get; set; }
        public UnitFacing Facing { get; set; }
        public decimal Area { get; set; }
        public int Rooms { get; set; }
        public decimal Price { get; set; }
        public int FloorId { get; set; }
        public int FloorNumber { get; set; }
        public int? BuyerId { get; set; }
        public int? BookingId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
