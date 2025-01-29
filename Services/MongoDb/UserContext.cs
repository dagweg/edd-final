using HouseRentalSystem.Models;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.Extensions.Options;

namespace HouseRentalSystem.Services.MongoDB;

public class UserContext : MongoContext<User>, IUserContext
{
    public UserContext(IOptions<MongoDbOptions> options)
        : base(options, "users") { }
}
