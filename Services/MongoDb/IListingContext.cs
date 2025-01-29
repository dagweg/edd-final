using HouseRentalSystem.Models;
using HouseRentalSystem.Services.MongoDB;

namespace HouseRentalSystem.Services.MongoDB;

public interface IListingContext : IMongoContext<Listing>
{
    // listing specific methods
}
