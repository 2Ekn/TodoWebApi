using TodoApi.DTOs;

namespace TodoApi.Services;
public interface IAuthService
{
    Task<string> LoginUserAsync(LoginUserDto user);
}