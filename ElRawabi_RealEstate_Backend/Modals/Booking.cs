using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public enum BookingStatus { Pending = 1, Confirmed = 2, Cancelled = 3 }

    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Buyer Buyer { get; set; }
        public int BuyerId { get; set; }
        public Unit Unit { get; set; }
        public int UnitId { get; set; }
    }
}