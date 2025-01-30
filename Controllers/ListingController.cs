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
        try
        {
            var listing = await _listingService.GetListingByIdAsync(id);
            return Ok(listing);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
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
    }
}
