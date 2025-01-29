using HouseRentalSystem.Controllers.Contracts;
using HouseRentalSystem.Models;

namespace HouseRentalSystem.Services;

public interface IListingService
{
    Task<List<Listing>> GetListingsAsync();
    Task<Listing> GetListingByIdAsync(string id);
    Task<List<Listing>> GetListingByNameAsync(string name);
    Task AddListingAsync(CreateListingRequest newListing, string userId);
    Task<Listing> UpdateListingAsync(string listingId, UpdateListingRequest updatedListing);
    Task<bool> DeleteListingAsync(string id);
}
