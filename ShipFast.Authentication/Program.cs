using Microsoft.EntityFrameworkCore;
using ShipFast.Authentication.DataAccess;
using ShipFast.Authentication.DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(
    builder.Services,
    builder.Configuration
);

var app = builder.Build();

app.MapPost("/authentication", async(AuthDbContext authDbContext, User user) =>
{
    await authDbContext.Users.AddAsync(user);
    await authDbContext.SaveChangesAsync();
    return Results.Created($"/authentication/{user.Id}", user);
}).Produces(StatusCodes.Status201Created);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
{
    services.AddDbContext<AuthDbContext>(
        opts =>
        {
            opts.UseNpgsql(configurationManager.GetConnectionString("AuthDb"));
        }, ServiceLifetime.Transient);
}