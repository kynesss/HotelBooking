using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Common.Presentation.Endpoints;

public static class EndpointMappingExtensions
{
    extension(WebApplication app)
    {
        public IApplicationBuilder MapEndpoints()
        {
            var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }

            return app;
        }
    }
}
