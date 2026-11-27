using System.Net;
using HotelBooking.Common.Domain;
using HotelBooking.Modules.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Modules.Users.Infrastructure.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{
    private const string PasswordCredentialType = "password";

    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Email,
            user.Email,
            user.FirstName,
            user.LastName,
            EmailVerified: true,
            Enabled: true,
            [new CredentialRepresentation(PasswordCredentialType, user.Password, Temporary: false)]);

        try
        {
            var identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogWarning(exception, "User registration failed");

            return Result.Failure<string>(IdentityProviderErrors.EmailIsNotUnique);
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.BadRequest)
        {
            logger.LogWarning(exception, "User registration failed");

            return Result.Failure<string>(IdentityProviderErrors.InvalidUserData);
        }
    }
}
