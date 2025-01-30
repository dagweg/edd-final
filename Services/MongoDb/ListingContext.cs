using HouseRentalSystem.Models;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseRentalSystem.Services.MongoDB
{
    public class ListingContext : MongoContext<Listing>, IListingContext
    {
        public ListingContext(IOptions<MongoDbOptions> options)
            : base(options, "listings") { }

        // If you want to add any custom behavior, you can override or add methods here

        // Example of custom method to get listings by name (if not already provided in MongoContext)
        public async Task<List<Listing>> GetByNameAsync(string name)
        {
            return await _collection.Find(l => l.Title.Contains(name)).ToListAsync();
        }

        // Example of custom method to get a listing by its ID (already exists in MongoContext)
        // public async Task<Listing> GetByIdAsync(string id)
        // {
        //     return await _collection.Find(l => l.Id == id).FirstOrDefaultAsync();
        // }
    }
}
