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

        public async Task<IEnumerable<BuyerResponseDto>> GetAllBuyersAsync() => _mapper.Map<IEnumerable<BuyerResponseDto>>(await _unitOfWork.Buyers.GetAllBuyersAsync());
        public async Task<BuyerResponseDto?> GetBuyerByIdAsync(int id) { var b = await _unitOfWork.Buyers.GetBuyerByIdAsync(id); return b == null ? null : _mapper.Map<BuyerResponseDto>(b); }

        public async Task<BuyerResponseDto> CreateBuyerAsync(BuyerRequestDto buyerDto, int? currentUserId)
        {
            var buyer = _mapper.Map<Buyer>(buyerDto);
            buyer.HashPassword = PasswordHelper.HashPassword(buyerDto.Password);
            await _unitOfWork.Buyers.AddBuyerAsync(buyer);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "عميل", buyer.Id, $"تم تسجيل عميل جديد: {buyer.FirstName} {buyer.LastName}", currentUserId);
            return _mapper.Map<BuyerResponseDto>(buyer);
        }

        public async Task<bool> UpdateBuyerAsync(int id, BuyerRequestDto buyerDto, int? currentUserId)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return false;
            _mapper.Map(buyerDto, buyer);
            _unitOfWork.Buyers.UpdateBuyer(buyer);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "عميل", id, $"تم تعديل بيانات العميل {buyer.FirstName}", currentUserId);
            return true;
        }

        public async Task<bool> DeleteBuyerAsync(int id, int? currentUserId)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return false;
            buyer.IsDeleted = true;
            _unitOfWork.Buyers.UpdateBuyer(buyer);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "عميل", id, $"تم حذف العميل {buyer.FirstName}", currentUserId);
            return true;
        }
    }
}