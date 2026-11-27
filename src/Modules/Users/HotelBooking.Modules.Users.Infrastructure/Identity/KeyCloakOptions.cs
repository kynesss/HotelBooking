namespace HotelBooking.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakOptions
{
    internal const string SectionName = "Users:KeyCloak";

    public required string AdminUrl { get; init; }

    public required string TokenUrl { get; init; }

    public required string ConfidentialClientId { get; init; }

    public required string ConfidentialClientSecret { get; init; }
}
