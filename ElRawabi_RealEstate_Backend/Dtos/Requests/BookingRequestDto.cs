using System.ComponentModel.DataAnnotations;
using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.DTOs.Requests
{
    public class BookingRequestDto
    {
        public BookingStatus Status { get; set; }
        [FutureDate]
        public DateTime? ExpectedCompletionDate { get; set; }
        public int BuyerId { get; set; }
        public int UnitId { get; set; }
    }
}
