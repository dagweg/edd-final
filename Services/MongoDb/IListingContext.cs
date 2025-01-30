using System.Collections.Generic;
using HouseRentalSystem.Models;

namespace HouseRentalSystem.Services.MongoDB
{
    public interface IListingContext : IMongoContext<Listing>
    {
        // Custom methods specific to Listing entity

        // Method to get listings by title
        Task<List<Listing>> GetByNameAsync(string name);
    }
}
