using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace HotelBooking.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakAuthDelegatingHandler(IOptions<KeyCloakOptions> options) : DelegatingHandler
{
    private readonly KeyCloakOptions _options = options.Value;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authToken = await GetAuthorizationTokenAsync(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken.AccessToken);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<AuthToken> GetAuthorizationTokenAsync(CancellationToken cancellationToken)
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _options.ConfidentialClientId),
            new("client_secret", _options.ConfidentialClientSecret),
            new("scope", "openid"),
            new("grant_type", "client_credentials")
        };

        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_options.TokenUrl));
        authRequest.Content = authRequestContent;

        using var authorizationResponse = await base.SendAsync(authRequest, cancellationToken);

        if (!authorizationResponse.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Keycloak token request failed with status code {(int)authorizationResponse.StatusCode}.");
        }

        return await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken)
               ?? throw new InvalidOperationException("Keycloak token endpoint returned an empty response.");
    }
}
