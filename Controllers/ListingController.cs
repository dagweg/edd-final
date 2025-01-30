using HouseRentalSystem.Controllers.Contracts;
using HouseRentalSystem.Services;
using HouseRentalSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentalSystem.Controllers;

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

        if (userId is null)
            return Unauthorized();

        await _listingService.AddListingAsync(createListingRequest, userId);

        return Created();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetListingById(string id)
    {
        var listing = await _listingService.GetListingByIdAsync(id);
        return Ok(listing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteListingById(string id)
    {
        var userId = JwtTokenHelper.GetUserId(User);
        if (userId is null)
            return Unauthorized();

        // First get the listing to check if the user is the owner
        try
        {
            var listing = await _listingService.GetListingByIdAsync(id);
            if (listing.HostId != userId)
                return Forbid("You can only delete your own listings");

            var deleted = await _listingService.DeleteListingAsync(id);
            if (!deleted)
                return NotFound($"Listing with ID {id} not found");

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Listing with ID {id} not found");
        }

        /// <summary>
    /// Gets a list of listings by a name.
    /// </summary>
    /// <param name="name">The name of the listing to get.</param>
    /// <returns>A list of listings with the name that was searched.</returns>
    [HttpGet("search/{name}")]
    public async Task<IActionResult> GetListingByName(string name)
    {
        var listings = await _listingService.GetListingByNameAsync(name);
        return Ok(listings);
    }
}
