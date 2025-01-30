using System.ComponentModel.DataAnnotations;
using HouseRentalSystem.Attributes;
using HouseRentalSystem.Controllers.Contracts;
using HouseRentalSystem.Helpers;
using HouseRentalSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentalSystem.Controllers;

/// <summary>
/// Controller for managing house rental listings.
/// </summary>
/// <response code="401">Unauthorized. User is not authenticated.</response>
/// <response code="500">Internal server error. Something went wrong on the server.</response>
[Authorize]
[ApiController]
[Route("listings")]
public class ListingController : ControllerBase
{
    private readonly IListingService _listingService;

    public ListingController(IListingService listingService)
    {
        _listingService = listingService;
    }

    /// <summary>
    /// Create a new house rental listing.
    /// </summary>
    /// <param name="createListingRequest"></param>
    /// <response code="201">The listing was successfully created.</response>
    /// <response code="401">Unauthorized. User is not authenticated.</response>
    /// <response code="500">Internal server error. Something went wrong on the server.</response>
    [HttpPost]
    public async Task<IActionResult> CreateListing(
        [FromBody] CreateListingRequest createListingRequest
    )
    {
        var userId = JwtTokenHelper.GetUserId(User);

        await _listingService.AddListingAsync(createListingRequest, userId);

        return Created();
    }

    /// <summary>
    /// Get all listings.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Returns all listings.</response>
    /// <response code="401">Unauthorized. User is not authenticated.</response>
    /// <response code="500">Internal server error. Something went wrong on the server.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetListingById(
        [FromRoute] [Required] [ObjectIdValidation] string id
    )
    {
        var listing = await _listingService.GetListingByIdAsync(id);
        return Ok(listing);
    }

    /// <summary>
    /// Delete a listing by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204">The listing was successfully deleted.</response>
    /// <response code="401">Unauthorized. User is not authenticated.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteListingById(
        [FromRoute] [Required] [ObjectIdValidation] string id
    )
    {
        var userId = JwtTokenHelper.GetUserId(User);

        await _listingService.DeleteListingAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Filters the house listings by different parameters.
    /// </summary>
    /// <param name="filterBy"></param>
    /// <param name="value"></param>
    /// <param name="paginationParameters"></param>
    /// <returns>A list of listings with the name that was searched.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> FilterListing(
        [FromQuery] string value,
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] string filterBy = "title"
    )
    {
        var listings = await _listingService.FilterListingAsync(
            filterBy,
            value,
            paginationParameters
        );
        return Ok(listings);
    }

    /// <summary>
    /// Update a listing by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="patchListingRequest"></param>
    /// <response code="200">The listing was successfully updated.</response>
    /// <response code="401">Unauthorized. User is not authenticated.</response>
    /// <response code="500">Internal server error. Something went wrong on the server.</response>
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchListing(
        [FromRoute] [Required] [ObjectIdValidation] string id,
        [FromBody] PatchListingRequest patchListingRequest
    )
    {
        var listing = await _listingService.PatchListingAsync(id, patchListingRequest);

        return Ok(listing);
    }

    /// <summary>
    /// Get all listings.
    /// </summary>
    /// <param name="paginationParameters"></param>
    /// <response code="200">Returns all listings.</response>
    /// <response code="401">Unauthorized. User is not authenticated.</response>
    /// <response code="500">Internal server error. Something went wrong on the server.</response>
    [HttpGet]
    public async Task<IActionResult> GetAllListings(
        [FromQuery] PaginationParameters paginationParameters
    )
    {
        var listing = await _listingService.GetListingsAsync(paginationParameters);

        return Ok(new { TotalCount = listing.Count, Listings = listing });
    }

    [HttpGet("host/{id}")]
    public async Task<IActionResult> GetListingByHostId(
        [FromRoute] [Required] [ObjectIdValidation] string id,
        [FromQuery] PaginationParameters paginationParameters
    )
    {
        var listing = await _listingService.GetListingsAsync(paginationParameters);

        return Ok(new { TotalCount = listing.Count, Listings = listing });
    }
}
