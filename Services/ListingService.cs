using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HouseRentalSystem.Controllers.Contracts;
using HouseRentalSystem.Models;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
                HostId = ObjectId.Parse(userId),
            };
            await _listingContext.CreateAsync(listing);
        }

        // Delete a listing by ID
        public async Task<bool> DeleteListingAsync(string id)
        {
            bool status = await _listingContext.DeleteAsync(id);
            if (status is false)
                throw new KeyNotFoundException("Listing not found!");
            return status;
        }

        // Get a listing by its ID
        public async Task<Listing> GetListingByIdAsync(string id)
        {
            var listing = await _listingContext.GetAsync(id);
            if (listing == null)
                throw new KeyNotFoundException("Listing not found!");

            return listing;
        }

        public async Task<List<Listing>> GetListingByHostIdAsync(string hostId)
        {
            var hostOid = ObjectId.Parse(hostId);
            var listing = await _listingContext.GetAllAsync(
                Builders<Listing>.Filter.Eq(l => l.HostId, hostOid)
            );
            if (listing == null)
                throw new KeyNotFoundException("Listing not found!");

            return listing;
        }

        public async Task<List<Listing>> FilterListingAsync(
            string filterBy,
            string value,
            PaginationParameters paginationParameters
        )
        {
            List<Listing> listings = [];

            if (filterBy.Equals(nameof(Listing.Title), StringComparison.OrdinalIgnoreCase))
            {
                listings = await _listingContext.FilterAsync(l => l.Title, value);
            }
            else if (filterBy.Equals(nameof(Listing.Location), StringComparison.OrdinalIgnoreCase))
            {
                listings = await _listingContext.FilterAsync(l => l.Location, value);
            }
            else if (
                filterBy.Equals(nameof(Listing.PricePerNight), StringComparison.OrdinalIgnoreCase)
            )
                listings = await _listingContext.FilterAsync(
                    l => l.PricePerNight,
                    decimal.Parse(value)
                );
            else if (
                filterBy.Equals(nameof(Listing.NumberOfGuests), StringComparison.OrdinalIgnoreCase)
            )
                listings = await _listingContext.FilterAsync(
                    l => l.NumberOfGuests,
                    int.Parse(value)
                );
            else
            {
                throw new ArgumentException(
                    $"Invalid filter parameter! Please filter by title, location, pricePerNight, numberOfGuests"
                );
            }

            var paginated = listings
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize);

            return paginated.ToList();
        }

        // Get all listings
        public async Task<List<Listing>> GetListingsAsync(PaginationParameters paginationParameters)
        {
            var listings = await _listingContext.GetAllAsync(null);
            return listings
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToList();
        }

        // Update an existing listing
        public async Task<Listing> PatchListingAsync(
            string listingId,
            PatchListingRequest patchedListing
        )
        {
            var existingListing = await _listingContext.GetAsync(listingId);
            if (existingListing == null)
                throw new KeyNotFoundException("Listing not found!");

            // Update the properties that are provided (null checks handled)
            existingListing.Title = patchedListing.Title ?? existingListing.Title;
            existingListing.Description = patchedListing.Description ?? existingListing.Description;
            existingListing.Location = patchedListing.Location ?? existingListing.Location;
            existingListing.PricePerNight =
                patchedListing.PricePerNight != 0
                    ? patchedListing.PricePerNight
                    : existingListing.PricePerNight;
            existingListing.NumberOfGuests =
                patchedListing.NumberOfGuests != 0
                    ? patchedListing.NumberOfGuests
                    : existingListing.NumberOfGuests;
            existingListing.ThumbnailUrl =
                patchedListing.ThumbnailUrl ?? existingListing.ThumbnailUrl;
            existingListing.Amenities = patchedListing.Amenities ?? existingListing.Amenities;

            // Update the listing in the database
            await _listingContext.UpdateAsync(listingId, existingListing);

            return existingListing;
        }
    }
}
