using Microsoft.EntityFrameworkCore;
using ShipFast.Order.DataAccess;
using ShipFast.Order.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(
    builder.Services,
    builder.Configuration
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// POST Add an order for delivery
app.MapPost("/orders", [Authorize] async (OrderDbContext orderDbContext, Orderr order) =>
{
    await orderDbContext.Orders.AddAsync(order);
    await orderDbContext.SaveChangesAsync();
    return Results.Created($"/orders/{order.Id}", order);
}).Produces(StatusCodes.Status201Created);

// GET All Orders
app.MapGet("/orders", [Authorize] async (OrderDbContext orderDbContext)
    => await orderDbContext.Orders.ToListAsync())
    .Produces<List<Orderr>>(StatusCodes.Status200OK);

// GET Order by OrderId
app.MapGet("/orders/{orderId}", [Authorize] async (OrderDbContext orderDbContext, int orderId) =>
{
    var order = await orderDbContext.Orders.FindAsync(orderId);
    return order != null ? Results.Ok(order) : Results.NotFound("Order not found.");
}).Produces<Orderr>(StatusCodes.Status200OK);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
{
    services.AddDbContext<OrderDbContext>(opts =>
        opts.UseNpgsql(configurationManager.GetConnectionString("OrderDb")),
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
