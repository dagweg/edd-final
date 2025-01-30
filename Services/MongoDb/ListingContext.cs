using HouseRentalSystem.Models;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HouseRentalSystem.Services.MongoDB
{
    public class ListingContext : MongoContext<Listing>, IListingContext
    {
        public ListingContext(IOptions<MongoDbOptions> options)
            : base(options, "listings") { }

        // If you want to add any custom behavior, you can override or add methods here

        public async Task<List<Listing>> GetByNameAsync(string name)
        {
            return await _collection.Find(l => l.Title.Contains(name)).ToListAsync();
        }
    }
}
