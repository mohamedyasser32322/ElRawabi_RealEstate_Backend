using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;
using ElRawabi_RealEstate_Backend.Helpers;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class BuyerService : IBuyerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public BuyerService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<BuyerResponseDto>> GetAllBuyersAsync()
        {
            var buyers = await _unitOfWork.Buyers.GetAllBuyersAsync();
            return _mapper.Map<IEnumerable<BuyerResponseDto>>(buyers);
        }

        public async Task<BuyerResponseDto?> GetBuyerByIdAsync(int id)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            return buyer == null ? null : _mapper.Map<BuyerResponseDto>(buyer);
        }

        public async Task<BuyerResponseDto> CreateBuyerAsync(BuyerRequestDto buyerDto)
        {
            var buyer = _mapper.Map<Buyer>(buyerDto);
            buyer.HashPassword = PasswordHelper.HashPassword(buyerDto.Password);
            await _unitOfWork.Buyers.AddBuyerAsync(buyer);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "عميل", buyer.Id, $"تم تسجيل عميل جديد: {buyer.FirstName} {buyer.LastName}", null);
            return _mapper.Map<BuyerResponseDto>(buyer);
        }

        public async Task<bool> UpdateBuyerAsync(int id, BuyerRequestDto buyerDto)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return false;
            _mapper.Map(buyerDto, buyer);
            _unitOfWork.Buyers.UpdateBuyer(buyer);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "عميل", id, $"تم تعديل بيانات العميل {buyer.FirstName}", null);
            return true;
        }

        public async Task<bool> DeleteBuyerAsync(int id)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return false;
            buyer.IsDeleted = true;
            _unitOfWork.Buyers.UpdateBuyer(buyer);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "عميل", id, $"تم حذف العميل {buyer.FirstName}", null);
            return true;
        }
    }
}
