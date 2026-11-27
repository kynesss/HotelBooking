using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Common.Infrastructure.Authentication;

internal static class AuthenticationConfiguration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAuthenticationInternal()
        {
            services.AddAuthorization();

            services.AddAuthentication().AddJwtBearer();

            services.AddHttpContextAccessor();

            services.ConfigureOptions<JwtBearerConfigureOptions>();

            return services;
        }
    }
}
