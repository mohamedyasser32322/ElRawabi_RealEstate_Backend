using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync() => _mapper.Map<IEnumerable<RoleResponseDto>>(await _unitOfWork.Roles.GetAllRolesAsync());
        public async Task<RoleResponseDto?> GetRoleByIdAsync(int id) => _mapper.Map<RoleResponseDto>(await _unitOfWork.Roles.GetRoleByIdAsync(id));

        public async Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto roleDto, int? currentUserId)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _unitOfWork.Roles.AddRoleAsync(role);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "دور", role.Id, $"تم إضافة دور جديد: {role.RoleName}", currentUserId);
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<bool> UpdateRoleAsync(int id, RoleRequestDto roleDto, int? currentUserId)
        {
            var role = await _unitOfWork.Roles.GetRoleByIdAsync(id);
            if (role == null) return false;
            _mapper.Map(roleDto, role);
            _unitOfWork.Roles.UpdateRole(role);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "دور", id, $"تم تعديل الدور: {role.RoleName}", currentUserId);
            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id, int? currentUserId)
        {
            var role = await _unitOfWork.Roles.GetRoleByIdAsync(id);
            if (role == null) return false;
            role.IsDeleted = true;
            _unitOfWork.Roles.UpdateRole(role);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "دور", id, $"تم حذف الدور: {role.RoleName}", currentUserId);
            return true;
        }
    }
}