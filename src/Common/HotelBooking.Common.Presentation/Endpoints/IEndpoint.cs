using Microsoft.AspNetCore.Routing;

namespace HotelBooking.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
