using System.ComponentModel.DataAnnotations;

namespace HouseRentalSystem.Controllers.Contracts;

public class CreateListingRequest
{
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    [MinLength(10)]
    public string Description { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;

    [Required]
    [Range(5, double.MaxValue)]
    public decimal PricePerNight { get; set; }

    [Required]
    [Range(1, 100)]
    public int NumberOfGuests { get; set; }
    public string? ThumbnailUrl { get; set; }

    [Required]
    public List<string> Amenities { get; set; } = new();
}
