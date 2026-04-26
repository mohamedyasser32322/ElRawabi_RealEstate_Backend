using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Helpers;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

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

        public async Task<IEnumerable<BuyerResponseDto>> GetAllBuyersAsync() =>
            _mapper.Map<IEnumerable<BuyerResponseDto>>(await _unitOfWork.Buyers.GetAllBuyersAsync());

        public async Task<BuyerResponseDto?> GetBuyerByIdAsync(int id)
        {
            var b = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            return b == null ? null : _mapper.Map<BuyerResponseDto>(b);
        }

        public async Task<BuyerResponseDto> CreateBuyerAsync(BuyerRequestDto buyerDto, int? currentUserId)
        {
            var buyer = _mapper.Map<Buyer>(buyerDto);
            buyer.HashPassword = PasswordHelper.HashPassword(buyerDto.Password);
            await _unitOfWork.Buyers.AddBuyerAsync(buyer);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { buyer.FirstName, buyer.LastName, buyer.Email, buyer.PhoneNumber };

            await _activityLogService.LogActivityAsync(
                "إضافة", "عميل", buyer.Id,
                $"تسجيل عميل جديد: {buyer.FirstName} {buyer.LastName}",
                currentUserId,
                newValues: newSnapshot);

            return _mapper.Map<BuyerResponseDto>(buyer);
        }

        public async Task<bool> UpdateBuyerAsync(int id, BuyerRequestDto buyerDto, int? currentUserId)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return false;

            var oldSnapshot = new { buyer.FirstName, buyer.LastName, buyer.Email, buyer.PhoneNumber };

            var currentPassword = buyer.HashPassword;
            _mapper.Map(buyerDto, buyer);
            buyer.HashPassword = !string.IsNullOrWhiteSpace(buyerDto.Password)
                ? PasswordHelper.HashPassword(buyerDto.Password)
                : currentPassword;

            _unitOfWork.Buyers.UpdateBuyer(buyer);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { buyer.FirstName, buyer.LastName, buyer.Email, buyer.PhoneNumber };

            await _activityLogService.LogActivityAsync(
                "تعديل", "عميل", id,
                $"تعديل بيانات العميل {buyer.FirstName} {buyer.LastName}",
                currentUserId,
                oldValues: oldSnapshot,
                newValues: newSnapshot);

            return true;
        }

        public async Task<bool> DeleteBuyerAsync(int id, int? currentUserId)
        {
            var buyer = await _unitOfWork.Buyers.GetBuyerByIdAsync(id);
            if (buyer == null) return false;

            var oldSnapshot = new { buyer.FirstName, buyer.LastName, buyer.Email, buyer.PhoneNumber };

            // ✅ جلب كل الوحدات المرتبطة بالعميل وتحريرها
            var buyerUnits = await _unitOfWork.Units.GetUnitsByBuyerIdAsync(id);
            var releasedCount = 0;

            foreach (var unit in buyerUnits)
            {
                // إلغاء الحجز المرتبط إن وجد
                if (unit.BookingId.HasValue)
                {
                    var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(unit.BookingId.Value);
                    if (booking != null)
                    {
                        booking.IsDeleted = true;
                        _unitOfWork.Bookings.UpdateBooking(booking);
                    }
                }

                // إرجاع الوحدة لحالة متاح
                unit.Status = UnitStatus.Available;
                unit.BuyerId = null;
                unit.BookingId = null;
                _unitOfWork.Units.UpdateUnit(unit);
                releasedCount++;
            }

            buyer.IsDeleted = true;
            _unitOfWork.Buyers.UpdateBuyer(buyer);
            await _unitOfWork.CompleteAsync();

            await _activityLogService.LogActivityAsync(
                "حذف", "عميل", id,
                $"حذف العميل {buyer.FirstName} {buyer.LastName}" +
                (releasedCount > 0 ? $" — تم تحرير {releasedCount} وحدة" : ""),
                currentUserId,
                oldValues: oldSnapshot);

            return true;
        }
    }
}