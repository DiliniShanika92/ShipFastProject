using Microsoft.EntityFrameworkCore;
using ShipFast.Delivery.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Configure authentication
ConfigureServices(
    builder.Services,
    builder.Configuration
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// GET delivery status by tracking code
app.MapGet("/deliveries/status/{trackingCode}", [AllowAnonymous] async (DeliveryDbContext deliveryDbContext, string trackingCode) =>
{
    var delivery = await deliveryDbContext.Deliveries.FirstOrDefaultAsync(d => d.TrackingCode == trackingCode);
    return delivery != null ? Results.Ok(delivery.Status) : Results.NotFound("Delivery not found.");
}).Produces<bool>(StatusCodes.Status200OK);

// POST cancel delivery
app.MapPost("/deliveries/cancel", [AllowAnonymous] async (DeliveryDbContext deliveryDbContext, int deliveryId) =>
{
    var delivery = await deliveryDbContext.Deliveries.FindAsync(deliveryId);
    if (delivery == null) return Results.NotFound("Delivery not found.");

    delivery.Status = false; // Assuming false indicates canceled
    await deliveryDbContext.SaveChangesAsync();
    return Results.Ok("Delivery canceled successfully.");
}).Produces(StatusCodes.Status200OK);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
{
    services.AddDbContext<DeliveryDbContext>(opts =>
        opts.UseNpgsql(configurationManager.GetConnectionString("DeliveryDb")),
        ServiceLifetime.Transient);

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("YourSecretKeyHere")),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    services.AddAuthorization();
}
