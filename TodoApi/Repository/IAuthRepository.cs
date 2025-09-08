using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Repository;
public interface IAuthRepository
{
    Task<string> LoginUserAsync(LoginUserDto loginUserDto);
}