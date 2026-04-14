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

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllRolesAsync();
            return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
        }

        public async Task<RoleResponseDto?> GetRoleByIdAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetRoleByIdAsync(id);
            if (role == null) return null;
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _unitOfWork.Roles.AddRoleAsync(role);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<bool> UpdateRoleAsync(int id, RoleRequestDto roleDto)
        {
            var role = await _unitOfWork.Roles.GetRoleByIdAsync(id);
            if (role == null) return false;

            _mapper.Map(roleDto, role);
            _unitOfWork.Roles.UpdateRole(role);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetRoleByIdAsync(id);
            if (role == null) return false;

            role.IsDeleted = true;
            _unitOfWork.Roles.UpdateRole(role);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
