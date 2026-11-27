using HotelBooking.Common.Infrastructure.Authentication;
using HotelBooking.Common.Infrastructure.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace HotelBooking.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(string databaseConnectionString)
        {
            services.AddAuthenticationInternal();

            var npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
            services.TryAddSingleton(npgsqlDataSource);

            services.TryAddSingleton<PublishDomainEventsInterceptor>();

            return services;
        }
    }
}
