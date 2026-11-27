using HotelBooking.Api.Extensions;
using HotelBooking.Common.Application;
using HotelBooking.Common.Infrastructure;
using HotelBooking.Common.Presentation.Endpoints;
using HotelBooking.Modules.Users.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddModuleConfiguration(["users"], builder.Environment.EnvironmentName);

builder.Services.AddApplication([HotelBooking.Modules.Users.Application.AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Database")!);

builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();
