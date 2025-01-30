using HouseRentalSystem.Controllers.Contracts;
using HouseRentalSystem.Services;
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
}
