using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services;
public interface IUserService
{
    Task<UserDto> AddAsync(CreateUserDto user);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<UserDto>?> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateUserDto user);
}