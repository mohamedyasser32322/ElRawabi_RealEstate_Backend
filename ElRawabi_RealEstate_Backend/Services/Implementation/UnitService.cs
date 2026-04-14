using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UnitResponseDto>> GetAllUnitsAsync()
        {
            var units = await _unitOfWork.Units.GetAllUnitsAsync();
            return _mapper.Map<IEnumerable<UnitResponseDto>>(units);
        }

        public async Task<UnitResponseDto?> GetUnitByIdAsync(int id)
        {
            var unit = await _unitOfWork.Units.GetUnitByIdAsync(id);
            if (unit == null) return null;
            return _mapper.Map<UnitResponseDto>(unit);
        }

        public async Task<UnitResponseDto> CreateUnitAsync(UnitRequestDto unitDto)
        {
            var unit = _mapper.Map<Unit>(unitDto);
            await _unitOfWork.Units.AddUnitAsync(unit);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<UnitResponseDto>(unit);
        }

        public async Task<bool> UpdateUnitAsync(int id, UnitRequestDto unitDto)
        {
            var unit = await _unitOfWork.Units.GetUnitByIdAsync(id);
            if (unit == null) return false;

            _mapper.Map(unitDto, unit);
            _unitOfWork.Units.UpdateUnit(unit);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteUnitAsync(int id)
        {
            var unit = await _unitOfWork.Units.GetUnitByIdAsync(id);
            if (unit == null) return false;

            unit.IsDeleted = true;
            _unitOfWork.Units.UpdateUnit(unit);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
