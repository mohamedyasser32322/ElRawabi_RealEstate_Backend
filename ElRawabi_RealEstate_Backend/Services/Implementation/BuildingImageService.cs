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

        public BuildingImageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BuildingImageResponseDto>> GetAllBuildingImagesAsync()
        {
            var images = await _unitOfWork.BuildingImages.GetAllBuildingImagesAsync();
            return _mapper.Map<IEnumerable<BuildingImageResponseDto>>(images);
        }

        public async Task<BuildingImageResponseDto?> GetBuildingImageByIdAsync(int id)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null) return null;
            return _mapper.Map<BuildingImageResponseDto>(image);
        }

        public async Task<BuildingImageResponseDto> CreateBuildingImageAsync(BuildingImageRequestDto imageDto)
        {
            var image = _mapper.Map<BuildingImage>(imageDto);
            await _unitOfWork.BuildingImages.AddBuildingImageAsync(image);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<BuildingImageResponseDto>(image);
        }

        public async Task<bool> UpdateBuildingImageAsync(int id, BuildingImageRequestDto imageDto)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null) return false;

            _mapper.Map(imageDto, image);
            _unitOfWork.BuildingImages.UpdateBuildingImage(image);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteBuildingImageAsync(int id)
        {
            var image = await _unitOfWork.BuildingImages.GetBuildingImageByIdAsync(id);
            if (image == null) return false;

            image.IsDeleted = true;
            _unitOfWork.BuildingImages.UpdateBuildingImage(image);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
