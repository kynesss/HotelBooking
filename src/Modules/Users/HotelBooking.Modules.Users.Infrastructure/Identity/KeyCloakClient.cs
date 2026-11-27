using System.Net.Http.Json;

namespace HotelBooking.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient)
{
    private const string UsersSegmentName = "users/";

    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        using var httpResponseMessage = await httpClient.PostAsJsonAsync("users", user, cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
    {
        var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery
                             ?? throw new InvalidOperationException("Location header is null");

        var userSegmentValueIndex = locationHeader.LastIndexOf(UsersSegmentName, StringComparison.OrdinalIgnoreCase);

        if (userSegmentValueIndex < 0)
        {
            throw new InvalidOperationException($"Location header '{locationHeader}' doesn't contain the users segment");
        }

        return locationHeader[(userSegmentValueIndex + UsersSegmentName.Length)..];
    }
}
