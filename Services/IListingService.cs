using HouseRentalSystem.Models;

namespace HouseRentalSystem.Services;

public interface IListingService
{
    Task<List<Listing>> GetListingsAsync();
    Task<Listing> GetListingByIdAsync(string id);
    Task<List<Listing>> GetListingByNameAsync(string name);
    Task<Listing> AddListingAsync(Listing newListing);
    Task<Listing> UpdateListingAsync(string id, Listing updatedListing);
    Task<bool> DeleteListingAsync(string id);
}
