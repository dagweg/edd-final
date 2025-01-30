using System.ComponentModel.DataAnnotations;

namespace HouseRentalSystem.Controllers.Contracts;

public class PatchListingRequest
{
    public string? Title { get; set; }

    [MinLength(10)]
    public string? Description { get; set; }

    public string? Location { get; set; }

    [Range(5, double.MaxValue)]
    public decimal PricePerNight { get; set; }

    [Range(1, 100)]
    public int NumberOfGuests { get; set; }
    public string? ThumbnailUrl { get; set; }

    public List<string>? Amenities { get; set; }
}
