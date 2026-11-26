using HotelBooking.Common.Application;
using HotelBooking.Common.Infrastructure;
using HotelBooking.Modules.Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication([HotelBooking.Modules.Users.Application.AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Database")!);

builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
