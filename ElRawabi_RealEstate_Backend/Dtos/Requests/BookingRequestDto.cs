using System.ComponentModel.DataAnnotations;
using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class BookingRequestDto
    {
        public BookingStatus Status { get; set; }
        [Range(0, double.MaxValue)]
        public decimal AmountPaid { get; set; }
        [Range(0, double.MaxValue)]
        public decimal RemainingAmount { get; set; }
        [FutureDate]
        public DateTime? ExpectedCompletionDate { get; set; }
        public int BuyerId { get; set; }
        public int UnitId { get; set; }
    }
}
