using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;

namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IBuyerService
    {
        Task<IEnumerable<BuyerResponseDto>> GetAllBuyersAsync();
        Task<BuyerResponseDto?> GetBuyerByIdAsync(int id);
        Task<BuyerResponseDto> CreateBuyerAsync(BuyerRequestDto buyerDto);
        Task<bool> UpdateBuyerAsync(int id, BuyerRequestDto buyerDto);
        Task<bool> DeleteBuyerAsync(int id);
    }
}
