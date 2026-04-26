using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class BuildingImageService : IBuildingImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public BuildingImageService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<BuildingImageResponseDto>> GetAllBuildingImagesAsync()
        {
            var images = await _unitOfWork.BuildingImages.GetAllBuildingImagesAsync();
            return _mapper.Map<IEnumerable<BuildingImageResponseDto>>(
                images.Where(img => !string.IsNullOrEmpty(img.ImageUrl) && File.Exists(img.ImageUrl)));
        }

        public async Task<BuildingImageResponseDto?> GetBuildingImageByIdAsync(int id)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null || string.IsNullOrEmpty(image.ImageUrl) || !File.Exists(image.ImageUrl)) return null;
            return _mapper.Map<BuildingImageResponseDto>(image);
        }

        public async Task<BuildingImageResponseDto> CreateBuildingImageAsync(
    BuildingImageRequestDto imageDto, int? currentUserId)
        {
            var image = _mapper.Map<BuildingImage>(imageDto);
            await _unitOfWork.BuildingImages.AddBuildingImageAsync(image);
            await _unitOfWork.CompleteAsync();

            // ✅ جلب اسم المبنى
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(image.BuildingId);

            var newSnapshot = new { image.BuildingId, image.ImageUrl };

            await _activityLogService.LogActivityAsync(
                "إضافة", "صورة مبنى", image.Id,
                $"إضافة صورة لمبنى {building?.Name ?? "—"}",  // ✅
                currentUserId,
                newValues: newSnapshot);

            return _mapper.Map<BuildingImageResponseDto>(image);
        }

        public async Task<bool> UpdateBuildingImageAsync(int id, BuildingImageRequestDto imageDto, int? currentUserId)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null) return false;

            // ✅ جلب اسم المبنى
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(image.BuildingId);

            var oldSnapshot = new { image.BuildingId, image.ImageUrl };

            _mapper.Map(imageDto, image);
            _unitOfWork.BuildingImages.UpdateBuildingImage(image);
            await _unitOfWork.CompleteAsync();

            var newSnapshot = new { image.BuildingId, image.ImageUrl };

            await _activityLogService.LogActivityAsync(
                "تعديل", "صورة مبنى", id,
                $"تعديل صورة مبنى {building?.Name ?? "—"}",  // ✅
                currentUserId,
                oldValues: oldSnapshot,
                newValues: newSnapshot);

            return true;
        }

        public async Task<bool> DeleteBuildingImageAsync(int id, int? currentUserId)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null) return false;

            // ✅ جلب اسم المبنى قبل الحذف
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(image.BuildingId);

            var oldSnapshot = new { image.BuildingId, image.ImageUrl };

            image.IsDeleted = true;
            _unitOfWork.BuildingImages.UpdateBuildingImage(image);
            await _unitOfWork.CompleteAsync();

            await _activityLogService.LogActivityAsync(
                "حذف", "صورة مبنى", id,
                $"حذف صورة مبنى {building?.Name ?? "—"}",  // ✅
                currentUserId,
                oldValues: oldSnapshot);

            return true;
        }
    }
}