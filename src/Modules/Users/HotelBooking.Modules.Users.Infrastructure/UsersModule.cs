using HotelBooking.Common.Application.Data;
using HotelBooking.Common.Infrastructure.Interceptors;
using HotelBooking.Common.Presentation.Endpoints;
using HotelBooking.Modules.Users.Application.Abstractions.Identity;
using HotelBooking.Modules.Users.Domain.Users;
using HotelBooking.Modules.Users.Infrastructure.Database;
using HotelBooking.Modules.Users.Infrastructure.Identity;
using HotelBooking.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HotelBooking.Modules.Users.Infrastructure;

public static class UsersModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddUsersModule(IConfiguration configuration)
        {
            services.AddEndpoints([Presentation.AssemblyReference.Assembly]);

            services.AddInfrastructure(configuration);

            return services;
        }

        private void AddInfrastructure(IConfiguration configuration)
        {
            services.Configure<KeyCloakOptions>(configuration.GetSection(KeyCloakOptions.SectionName));

            services.AddTransient<KeyCloakAuthDelegatingHandler>();

            services
                .AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
                {
                    var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;

                    httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
                })
                .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

            services.AddTransient<IIdentityProviderService, IdentityProviderService>();

            services.AddDbContext<UsersDbContext>((sp, options) =>
                options
                    .UseNpgsql(
                        configuration.GetConnectionString("Database"),
                        npgsqlOptions => npgsqlOptions
                            .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users))
                    .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>())
                    .UseSnakeCaseNamingConvention());

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());
        }
    }
}
