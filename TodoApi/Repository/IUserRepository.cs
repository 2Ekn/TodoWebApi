using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Repository;
public interface IUserRepository
{
    Task<UserDto> AddAsync(CreateUserDto user);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<UserDto>?> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateUserDto userToUpdate);
}