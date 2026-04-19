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
            return _mapper.Map<IEnumerable<BuildingImageResponseDto>>(images.Where(img => !string.IsNullOrEmpty(img.ImageUrl) && File.Exists(img.ImageUrl)));
        }

        public async Task<BuildingImageResponseDto?> GetBuildingImageByIdAsync(int id)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null || string.IsNullOrEmpty(image.ImageUrl) || !File.Exists(image.ImageUrl)) return null;
            return _mapper.Map<BuildingImageResponseDto>(image);
        }

        public async Task<BuildingImageResponseDto> CreateBuildingImageAsync(BuildingImageRequestDto imageDto, int? currentUserId)
        {
            var image = _mapper.Map<BuildingImage>(imageDto);
            await _unitOfWork.BuildingImages.AddBuildingImageAsync(image);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "صورة", image.Id, $"تم إضافة صورة للعمارة {image.BuildingId}", currentUserId);
            return _mapper.Map<BuildingImageResponseDto>(image);
        }

        public async Task<bool> UpdateBuildingImageAsync(int id, BuildingImageRequestDto imageDto, int? currentUserId)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null) return false;
            _mapper.Map(imageDto, image);
            _unitOfWork.BuildingImages.UpdateBuildingImage(image);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "صورة", id, $"تم تعديل بيانات الصورة رقم {id}", currentUserId);
            return true;
        }

        public async Task<bool> DeleteBuildingImageAsync(int id, int? currentUserId)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null) return false;
            image.IsDeleted = true;
            _unitOfWork.BuildingImages.UpdateBuildingImage(image);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "صورة", id, $"تم حذف الصورة رقم {id}", currentUserId);
            return true;
        }
    }
}