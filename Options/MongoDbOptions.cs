namespace HouseRentalSystem.Options;

public class MongoDbOptions
{
    public static string SectionName = "MongoDbOptions";
    public string? ConnectionString { get; init; }
    public string? DatabaseName { get; init; }
}
