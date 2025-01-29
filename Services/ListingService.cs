using HouseRentalSystem.Models;

namespace HouseRentalSystem.Services;

public class ListingService : IListingService
{
    public Task<Listing> AddListingAsync(Listing newListing)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteListingAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Listing> GetListingByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Listing>> GetListingByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<List<Listing>> GetListingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Listing> UpdateListingAsync(string id, Listing updatedListing)
    {
        throw new NotImplementedException();
    }
}
