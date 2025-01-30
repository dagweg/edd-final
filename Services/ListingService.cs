using HouseRentalSystem.Controllers.Contracts;
using HouseRentalSystem.Models;
using HouseRentalSystem.Services.MongoDB;

namespace HouseRentalSystem.Services;

public class ListingService : IListingService
{
    private readonly IListingContext _listingContext;
    private readonly IUserContext _userContext;

    public ListingService(IListingContext listingContext, IUserContext userContext)
    {
        _listingContext = listingContext;
        _userContext = userContext;
    }

    public async Task AddListingAsync(CreateListingRequest newListing, string userId)
    {
        var hostId = await _userContext.GetAsync(userId);

        if (hostId is null)
            throw new KeyNotFoundException("User not found!");

        // create a listing if the user exists
        await _listingContext.CreateAsync(
            new Listing
            {
                Title = newListing.Title,
                Description = newListing.Description,
                Location = newListing.Location,
                PricePerNight = newListing.PricePerNight,
                NumberOfGuests = newListing.NumberOfGuests,
                ThumbnailUrl = newListing.ThumbnailUrl,
                Amenities = newListing.Amenities,
                HostId = userId,
            }
        );
    }

    public Task<bool> DeleteListingAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Listing> GetListingByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Listing>> GetListingByNameAsync(string name)
    {
        // Retrieve all listings
        var allListings = await _listingContext.GetAllAsync();

        // Filter in memory
        var filteredListings = allListings
            .Where(l => l.Title.ToLower().Contains(name.ToLower()))
            .ToList();

        return filteredListings;
    }

    public Task<List<Listing>> GetListingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Listing> UpdateListingAsync(string listingId, UpdateListingRequest updatedListing)
    {
        throw new NotImplementedException();
    }
}
