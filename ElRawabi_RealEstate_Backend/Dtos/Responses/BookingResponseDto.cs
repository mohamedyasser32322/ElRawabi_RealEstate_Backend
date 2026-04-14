using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }
        public int BuyerId { get; set; }
        public string BuyerFullName { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string UnitNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
