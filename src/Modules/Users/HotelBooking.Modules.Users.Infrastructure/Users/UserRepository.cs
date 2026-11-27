using HotelBooking.Modules.Users.Domain.Users;
using HotelBooking.Modules.Users.Infrastructure.Database;

namespace HotelBooking.Modules.Users.Infrastructure.Users;

internal sealed class UserRepository(UsersDbContext dbContext) : IUserRepository
{
    public void Insert(User user)
    {
        dbContext.Users.Add(user);
    }
}
