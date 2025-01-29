namespace HouseRentalSystem.Options;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";

    public required int ExpiryMinutes { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SecretKey { get; init; }
}
