using HouseRentalSystem.Models;

namespace HouseRentalSystem.Services;

public interface IUserService
{
    Task AddUserAsync(User newUser);
    Task<User> UpdateUserAsync(string id, User updatedUser);
    Task<bool> DeleteUserAsync(string id);
}
