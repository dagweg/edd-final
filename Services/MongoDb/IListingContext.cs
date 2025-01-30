using HouseRentalSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseRentalSystem.Services.MongoDB
{
    public interface IListingContext : IMongoContext<Listing>
    {
        // Custom methods specific to Listing entity

        // Method to get listings by title
        Task<List<Listing>> GetByNameAsync(string name);

        // Method to get a listing by its ID (inherited from IMongoContext)
        // Task<Listing> GetByIdAsync(string id);  // You can use the inherited method

        // You can also add other custom queries if necessary, e.g., filtering by location, price, etc.
    }
}
