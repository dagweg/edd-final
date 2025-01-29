using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HouseRentalSystem.Models;

public class Listing
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Location { get; set; }
    public decimal PricePerNight { get; set; }
    public int NumberOfGuests { get; set; }
    public string? ThumbnailUrl { get; set; }
    public List<string> Amenities { get; set; } = [];
    public required string HostId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
