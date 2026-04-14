using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class BuyerService : IBuyerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BuyerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BuyerResponseDto>> GetAllBuyersAsync()
        {
            var buyers = await _unitOfWork.Buyers.GetAllBuyersAsync();
            return _mapper.Map<IEnumerable<BuyerResponseDto>>(buyers);
        }

        public async Task<BuyerResponseDto?> GetBuyerByIdAsync(int id)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return null;
            return _mapper.Map<BuyerResponseDto>(buyer);
        }

        public async Task<BuyerResponseDto> CreateBuyerAsync(BuyerRequestDto buyerDto)
        {
            var buyer = _mapper.Map<Buyer>(buyerDto);
            await _unitOfWork.Buyers.AddBuyerAsync(buyer);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<BuyerResponseDto>(buyer);
        }

        public async Task<bool> UpdateBuyerAsync(int id, BuyerRequestDto buyerDto)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return false;

            _mapper.Map(buyerDto, buyer);
            _unitOfWork.Buyers.UpdateBuyer(buyer);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteBuyerAsync(int id)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return false;

            buyer.IsDeleted = true;
            _unitOfWork.Buyers.UpdateBuyer(buyer);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
