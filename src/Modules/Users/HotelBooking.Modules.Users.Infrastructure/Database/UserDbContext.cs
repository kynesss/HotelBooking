using HotelBooking.Common.Application.Data;
using HotelBooking.Modules.Users.Domain.Users;
using HotelBooking.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Modules.Users.Infrastructure.Database;

internal sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
