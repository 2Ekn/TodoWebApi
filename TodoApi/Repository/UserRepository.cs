using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Repository;
public class UserRepository : IUserRepository
{
    private readonly TodoDbContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(TodoDbContext dbContext, ILogger<UserRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<UserDto> AddAsync(CreateUserDto createUser)
    {
        try
        {
            var user = new User
            {
                Username = createUser.UserName,
                Password = createUser.Password
            };

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };
        }
        catch (DbException e)
        {
            _logger.LogError(e, "Database error while adding user.");
            return null!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while adding user.");
            return null!;
        }
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        try
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                _logger.LogError("Did not find User with ID: {UserID}", id);
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };
        }
        catch (DbException e)
        {
            _logger.LogError(e, "Database error while retrieving user {UserId}", id);
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while retrieving user {UserId}", id);
            return null;
        }
    }

    public async Task<IEnumerable<UserDto>?> GetAllAsync()
    {
        try
        {
            var users = await _dbContext.Users
                .AsNoTracking()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username
                })
                .ToListAsync();

            return users;
        }
        catch (DbException e)
        {
            _logger.LogError(e, "Database error while retrieving users. ");
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected error while retrieving users.");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDto updateUserDto)
    {
        try
        {
            var userFromDb = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userFromDb is null)
            {
                _logger.LogError("Did not find User with ID: {UserID}", id);
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateUserDto.Username))
            {
                _logger.LogWarning("Can't leave username empty.");
                return false;
            }

            var usernameTaken = await _dbContext.Users
                .AnyAsync(u => u.Username == updateUserDto.Username);

            if (usernameTaken)
            {
                _logger.LogError("Username already taken!");
                return false;
            }

            userFromDb.Username = updateUserDto.Username;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully updated user {UserId}", id);

            return true;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error while updating user {UserId}. ", id);
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error updating user {UserId}. ", id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var userFromDb = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userFromDb is null)
            {
                _logger.LogWarning("Could not find user with ID: {UserId}", id);
                return false;
            }

            _dbContext.Users.Remove(userFromDb);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError(e, "Concurrency error while deleting user {UserId}.", id);
            return false;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error while deleting user {UserId}. ", id);
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error deleting user {UserId}. ", id);
            return false;
        }
    }
}