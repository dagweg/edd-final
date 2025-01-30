using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HouseRentalSystem.Controllers.Contracts;
using HouseRentalSystem.Models;
using HouseRentalSystem.Services.MongoDB;
using MongoDB.Driver;

namespace HouseRentalSystem.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingContext _listingContext;
        private readonly IUserContext _userContext;

        public ListingService(IListingContext listingContext, IUserContext userContext)
        {
            _listingContext = listingContext;
            _userContext = userContext;
        }

        // Add a new listing
        public async Task AddListingAsync(CreateListingRequest newListing, string userId)
        {
            var host = await _userContext.GetAsync(userId);
            if (host == null)
                throw new KeyNotFoundException("User not found!");

            // Create the listing and add it to the collection
            var listing = new Listing
            {
                Title = newListing.Title,
                Description = newListing.Description,
                Location = newListing.Location,
                PricePerNight = newListing.PricePerNight,
                NumberOfGuests = newListing.NumberOfGuests,
                ThumbnailUrl = newListing.ThumbnailUrl,
                Amenities = newListing.Amenities ?? new List<string>(), // Ensure Amenities isn't null
                HostId = userId,
            };

            await _listingContext.CreateAsync(listing);
        }

        // Delete a listing by ID
        public async Task<bool> DeleteListingAsync(string id)
        {
            return await _listingContext.DeleteAsync(id);
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
        
        // Get a listing by its ID
        public async Task<Listing> GetListingByIdAsync(string id)
        {
            var listing = await _listingContext.GetAsync(id);
            if (listing == null)
                throw new KeyNotFoundException("Listing not found!");


            return listing;
        }

        // Get listings by name (title)
        public async Task<List<Listing>> GetListingByNameAsync(string name)
        {
            return await _listingContext.GetByNameAsync(name);
        }

        // Get all listings
        public async Task<List<Listing>> GetListingsAsync()
        {
            return await _listingContext.GetAllAsync();
        }

        // Update an existing listing
        public async Task<Listing> UpdateListingAsync(
            string listingId,
            UpdateListingRequest updatedListing
        )
        {
            var existingListing = await _listingContext.GetAsync(listingId);
            if (existingListing == null)
                throw new KeyNotFoundException("Listing not found!");

            // Update the properties that are provided (null checks handled)
            existingListing.Title = updatedListing.Title ?? existingListing.Title;
            existingListing.Description = updatedListing.Description ?? existingListing.Description;
            existingListing.Location = updatedListing.Location ?? existingListing.Location;
            existingListing.PricePerNight =
                updatedListing.PricePerNight != 0
                    ? updatedListing.PricePerNight
                    : existingListing.PricePerNight;
            existingListing.NumberOfGuests =
                updatedListing.NumberOfGuests != 0
                    ? updatedListing.NumberOfGuests
                    : existingListing.NumberOfGuests;
            existingListing.ThumbnailUrl =
                updatedListing.ThumbnailUrl ?? existingListing.ThumbnailUrl;
            existingListing.Amenities = updatedListing.Amenities ?? existingListing.Amenities;

            // Update the listing in the database
            await _listingContext.UpdateAsync(listingId, existingListing);

            return existingListing;
        }
    }
}
