using HotelBooking.Common.Domain;

namespace HotelBooking.Modules.Users.Application.Abstractions.Identity;

public static class IdentityProviderErrors
{
    public static readonly Error EmailIsNotUnique = Error.Conflict(
        "Identity.EmailIsNotUnique",
        "The specified email is not unique.");

    public static readonly Error InvalidUserData = Error.Problem(
        "Identity.InvalidUserData",
        "The identity provider rejected the user data.");
}
