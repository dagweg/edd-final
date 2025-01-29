using HouseRentalSystem.Models;
using HouseRentalSystem.Services.MongoDB;

namespace HouseRentalSystem.Services.MongoDB;

public interface IUserContext : IMongoContext<User>
{
    // user specific methods
    Task<User?> GetUserByEmailAsync(string email);
}
