using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;

    public AuthService(IAuthRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> LoginUserAsync(LoginUserDto user)
       => await _repository.LoginUserAsync(user);



}
