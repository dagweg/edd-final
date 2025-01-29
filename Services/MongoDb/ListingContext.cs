using HouseRentalSystem.Models;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.Extensions.Options;

namespace HouseRentalSystem.Services.MongoDB;

public class ListingContext : MongoContext<Listing>, IListingContext
{
    public ListingContext(IOptions<MongoDbOptions> options)
        : base(options, "listings") { }
}
