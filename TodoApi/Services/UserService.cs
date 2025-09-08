using BCrypt.Net;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserDto> AddAsync(CreateUserDto createUser)
    {
        if (string.IsNullOrWhiteSpace(createUser.UserName))
        {
            _logger.LogWarning("Username can't be empty.");
            return null!;
        }

        createUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(createUser.Password);

        var userDto = await _userRepository.AddAsync(createUser);

        if (userDto is null)
        {
            _logger.LogWarning("Something went wrong when adding user.");
            return null!;
        }

        return userDto;
    }

    public async Task<UserDto?> GetByIdAsync(int id)
        => await _userRepository.GetByIdAsync(id);

    public async Task<IEnumerable<UserDto>?> GetAllAsync()
        => await _userRepository.GetAllAsync();

    public async Task<bool> UpdateAsync(int id, UpdateUserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            _logger.LogWarning("Username can't be empty.");
            return false;
        }

        await _userRepository.UpdateAsync(id, user);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
        => await _userRepository.DeleteAsync(id);

}
