using HouseRentalSystem.Controllers.Contracts;
using HouseRentalSystem.Models;

namespace HouseRentalSystem.Services;

public interface IListingService
{
    Task<List<Listing>> GetListingsAsync(PaginationParameters paginationParameters);
    Task<Listing> GetListingByIdAsync(string id);
    Task<List<Listing>> GetListingByHostIdAsync(string hostId);
    Task<List<Listing>> FilterListingAsync(
        string filterBy,
        string value,
        PaginationParameters paginationParameters
    );
    Task AddListingAsync(CreateListingRequest newListing, string userId);
    Task<Listing> PatchListingAsync(string listingId, PatchListingRequest updatedListing);
    Task<bool> DeleteListingAsync(string id);
}
