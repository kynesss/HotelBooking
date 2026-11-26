using HotelBooking.Common.Domain;

namespace HotelBooking.Modules.Users.Domain.Users;

public class User : Entity
{
    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string IdentityId { get; private set; }
}
