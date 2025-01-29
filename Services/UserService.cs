using HouseRentalSystem.Exceptions;
using HouseRentalSystem.Models;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.AspNetCore.Identity;

namespace HouseRentalSystem.Services;

public class UserService : IUserService
{
    private readonly IUserContext _userContext;
    private readonly IPasswordHasher<string> _passwordHasher;

    public UserService(IUserContext userContext, IPasswordHasher<string> passwordHasher)
    {
        _userContext = userContext;
        _passwordHasher = passwordHasher;
    }

    public async Task AddUserAsync(User newUser)
    {
        if (await _userContext.GetAsync(newUser.Id.ToString()) != null)
        {
            throw new AlreadyExistsException("User already exists.");
        }
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser.Email, newUser.PasswordHash);
        await _userContext.CreateAsync(newUser);
    }

    public Task<bool> DeleteUserAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateUserAsync(string id, User updatedUser)
    {
        throw new NotImplementedException();
    }
}
