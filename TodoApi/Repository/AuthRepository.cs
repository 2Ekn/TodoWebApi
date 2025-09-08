using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;

namespace TodoApi.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly TodoDbContext _dbContext;
    private readonly TokenProvider _tokenProvider;

    public AuthRepository(TodoDbContext dbContext, TokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<string> LoginUserAsync(LoginUserDto loginUserDto)
    {
        var user = await _dbContext.Users
            .Where(u => u.Username == loginUserDto.Username)
            .FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(loginUserDto.Password, user.Password))
        {
            return null!;
        }

        var token = _tokenProvider.Create(user);

        return token;
    }


}
