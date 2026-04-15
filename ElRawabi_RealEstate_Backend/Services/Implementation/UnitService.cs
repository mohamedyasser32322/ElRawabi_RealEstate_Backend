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
        private readonly IActivityLogService _activityLogService;

        public UnitService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<UnitResponseDto>> GetAllUnitsAsync()
        {
            var units = await _unitOfWork.Units.GetAllUnitsAsync();
            return _mapper.Map<IEnumerable<UnitResponseDto>>(units);
        }

        public async Task<UnitResponseDto?> GetUnitByIdAsync(int id)
        {
            var unit = await _unitOfWork.Units.GetUnitByIdAsync(id);
            return unit == null ? null : _mapper.Map<UnitResponseDto>(unit);
        }

        public async Task<UnitResponseDto> CreateUnitAsync(UnitRequestDto unitDto)
        {
            var unit = _mapper.Map<Unit>(unitDto);
            await _unitOfWork.Units.AddUnitAsync(unit);

            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(unitDto.FloorId);
            if (floor != null)
            {
                var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(floor.BuildingId);
                if (building != null)
                {
                    building.TotalUnits++;
                    building.AvailableUnits++;
                    var project = await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId);
                    if (project != null) { project.TotalUnits++; project.AvailableUnits++; }
                }
            }

            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "وحدة", unit.Id, $"تم إضافة وحدة رقم {unit.UnitNumber}", null);
            return _mapper.Map<UnitResponseDto>(unit);
        }

        public async Task<bool> UpdateUnitAsync(int id, UnitRequestDto unitDto)
        {
            var unit = await _unitOfWork.Units.GetUnitByIdAsync(id);
            if (unit == null) return false;
            _mapper.Map(unitDto, unit);
            _unitOfWork.Units.UpdateUnit(unit);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "وحدة", id, $"تم تعديل بيانات الوحدة {unit.UnitNumber}", null);
            return true;
        }

        public async Task<bool> DeleteUnitAsync(int id)
        {
            var unit = await _unitOfWork.Units.GetUnitByIdAsync(id);
            if (unit == null) return false;
            unit.IsDeleted = true;

            var floor = await _unitOfWork.Floors.GetFloorByIdAsync(unit.FloorId);
            if (floor != null)
            {
                var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(floor.BuildingId);
                if (building != null)
                {
                    building.TotalUnits--;
                    if (unit.Status == UnitStatus.Available) building.AvailableUnits--;
                    var project = await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId);
                    if (project != null) { project.TotalUnits--; if (unit.Status == UnitStatus.Available) project.AvailableUnits--; }
                }
            }

            _unitOfWork.Units.UpdateUnit(unit);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "وحدة", id, $"تم حذف الوحدة {unit.UnitNumber}", null);
            return true;
        }
    }
}
