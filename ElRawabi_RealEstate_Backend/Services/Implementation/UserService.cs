using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;
using ElRawabi_RealEstate_Backend.Helpers;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IActivityLogService activityLogService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            return user == null ? null : _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto userDto, int? currentUserId)
        {
            var user = _mapper.Map<User>(userDto);
            user.HashPassword = PasswordHelper.HashPassword(userDto.Password);
            await _unitOfWork.Users.AddUserAsync(user);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("إضافة", "مستخدم", user.Id, $"تم إضافة مستخدم جديد: {user.FirstName} {user.LastName}", currentUserId);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateRequestDto userDto, int? currentUserId)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null) return false;
            _mapper.Map(userDto, user);
            _unitOfWork.Users.UpdateUser(user);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("تعديل", "مستخدم", id, $"تم تعديل بيانات المستخدم {user.FirstName}", currentUserId);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id, int? currentUserId)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null) return false;
            user.IsDeleted = true;
            _unitOfWork.Users.UpdateUser(user);
            await _unitOfWork.CompleteAsync();
            await _activityLogService.LogActivityAsync("حذف", "مستخدم", id, $"تم حذف المستخدم {user.FirstName}", currentUserId);
            return true;
        }
    }
}