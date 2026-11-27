using System.Text.Json.Serialization;

namespace HotelBooking.Modules.Users.Infrastructure.Identity;

internal sealed record AuthToken([property: JsonPropertyName("access_token")] string AccessToken);
