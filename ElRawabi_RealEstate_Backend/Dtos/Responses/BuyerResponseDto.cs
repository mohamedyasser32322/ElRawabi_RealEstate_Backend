namespace ElRawabi_RealEstate_Backend.DTOs.Responses
{
    public class BuyerResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? NationalId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
