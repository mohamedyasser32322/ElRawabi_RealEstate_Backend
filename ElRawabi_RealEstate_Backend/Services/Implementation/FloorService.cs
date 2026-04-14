using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class FloorService : IFloorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FloorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FloorResponseDto>> GetAllFloorsAsync()
        {
            var floors = await _unitOfWork.Floors.GetAllFloorsAsync();
            return _mapper.Map<IEnumerable<FloorResponseDto>>(floors);
        }

        public async Task<FloorResponseDto?> GetFloorByIdAsync(int id)
        {
            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(id);
            if (floor == null) return null;
            return _mapper.Map<FloorResponseDto>(floor);
        }

        public async Task<FloorResponseDto> CreateFloorAsync(FloorRequestDto floorDto)
        {
            var floor = _mapper.Map<Floor>(floorDto);
            await _unitOfWork.Floors.AddFloorAsync(floor);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<FloorResponseDto>(floor);
        }

        public async Task<bool> UpdateFloorAsync(int id, FloorRequestDto floorDto)
        {
            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(id);
            if (floor == null) return false;

            _mapper.Map(floorDto, floor);
            _unitOfWork.Floors.UpdateFloor(floor);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteFloorAsync(int id)
        {
            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(id);
            if (floor == null) return false;

            floor.IsDeleted = true;
            _unitOfWork.Floors.UpdateFloor(floor);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
