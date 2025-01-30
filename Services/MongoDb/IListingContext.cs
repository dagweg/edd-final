using System.Collections.Generic;
using System.Linq.Expressions;
using HouseRentalSystem.Models;

namespace HouseRentalSystem.Services.MongoDB
{
    public interface IListingContext : IMongoContext<Listing>
    {
        // Custom methods specific to Listing entity


        Task<List<Listing>> FilterAsync(Expression<Func<Listing, object>> filterBy, string value);
        Task<List<Listing>> FilterAsync(Expression<Func<Listing, object>> filterBy, int value);
        Task<List<Listing>> FilterAsync(Expression<Func<Listing, object>> filterBy, decimal value);
    }
}
