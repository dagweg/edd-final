using System.Linq.Expressions;
using HouseRentalSystem.Models;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HouseRentalSystem.Services.MongoDB
{
    public class ListingContext : MongoContext<Listing>, IListingContext
    {
        public ListingContext(IOptions<MongoDbOptions> options)
            : base(options, "listings") { }

        /// <summary>
        /// Filter listings by a specific field
        /// </summary>
        /// <param name="filterBy"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<List<Listing>> FilterAsync(
            Expression<Func<Listing, object>> filterBy,
            string value
        )
        {
            return await (
                _collection.Find(
                    Builders<Listing>.Filter.Regex(
                        filterBy,
                        new BsonRegularExpression($"/{value}/i")
                    )
                )
            ).ToListAsync();
        }

        public async Task<List<Listing>> FilterAsync(
            Expression<Func<Listing, object>> filterBy,
            int value
        )
        {
            return await (
                _collection.Find(Builders<Listing>.Filter.Eq(filterBy, value))
            ).ToListAsync();
        }

        public async Task<List<Listing>> FilterAsync(
            Expression<Func<Listing, object>> filterBy,
            decimal value
        )
        {
            return await (
                _collection.Find(Builders<Listing>.Filter.Eq(filterBy, value))
            ).ToListAsync();
        }
    }
}
