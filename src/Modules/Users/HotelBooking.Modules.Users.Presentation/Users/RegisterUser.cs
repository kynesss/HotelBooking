using HotelBooking.Common.Presentation.Endpoints;
using HotelBooking.Common.Presentation.Results;
using HotelBooking.Modules.Users.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace HotelBooking.Modules.Users.Presentation.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new RegisterUserCommand(request.Email, request.Password, request.FirstName, request.LastName),
                    cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Users)
            .WithName(nameof(RegisterUser))
            .WithSummary("Register a user")
            .WithDescription("Creates an account in the identity provider and the matching user profile.")
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private sealed record Request(string Email, string Password, string FirstName, string LastName);
}
