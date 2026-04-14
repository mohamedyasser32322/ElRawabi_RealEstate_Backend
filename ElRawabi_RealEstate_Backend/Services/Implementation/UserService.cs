using AutoMapper;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null) return null;
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.HashPassword = userDto.Password; // In real app, hash this
            await _unitOfWork.Users.AddUserAsync(user);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateRequestDto userDto)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null) return false;

            _mapper.Map(userDto, user);
            _unitOfWork.Users.UpdateUser(user);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null) return false;

            user.IsDeleted = true;
            _unitOfWork.Users.UpdateUser(user);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
