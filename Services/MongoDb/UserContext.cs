using HouseRentalSystem.Models;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HouseRentalSystem.Services.MongoDB;

public class UserContext : MongoContext<User>, IUserContext
{
    public UserContext(IOptions<MongoDbOptions> options)
        : base(options, "users") { }

    // looks for the user by email, if it doesn't find it, returns null
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _collection
            .Find(Builders<User>.Filter.Eq(p => p.Email, email))
            .FirstOrDefaultAsync();
    }
}
