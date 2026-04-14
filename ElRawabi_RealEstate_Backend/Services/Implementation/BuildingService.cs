using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class BuildingService : IBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BuildingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BuildingResponseDto>> GetAllBuildingsAsync()
        {
            var buildings = await _unitOfWork.Buildings.GetAllBuildingsAsync();
            return _mapper.Map<IEnumerable<BuildingResponseDto>>(buildings);
        }

        public async Task<BuildingResponseDto?> GetBuildingByIdAsync(int id)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return null;
            return _mapper.Map<BuildingResponseDto>(building);
        }

        public async Task<BuildingResponseDto> CreateBuildingAsync(BuildingRequestDto buildingDto)
        {
            var building = _mapper.Map<Building>(buildingDto);
            await _unitOfWork.Buildings.AddBuildingAsync(building);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<BuildingResponseDto>(building);
        }

        public async Task<bool> UpdateBuildingAsync(int id, BuildingRequestDto buildingDto)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return false;

            _mapper.Map(buildingDto, building);
            _unitOfWork.Buildings.UpdateBuilding(building);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteBuildingAsync(int id)
        {
            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(id);
            if (building == null) return false;

            building.IsDeleted = true;
            _unitOfWork.Buildings.UpdateBuilding(building);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
