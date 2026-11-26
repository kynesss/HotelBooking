using HotelBooking.Modules.Users.Domain.Users;
using HotelBooking.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Modules.Users.Infrastructure.Database;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    internal DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
