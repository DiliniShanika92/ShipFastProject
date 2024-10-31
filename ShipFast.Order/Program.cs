using Microsoft.EntityFrameworkCore;
using ShipFast.Order.DataAccess;
using ShipFast.Order.DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(
    builder.Services,
    builder.Configuration
);

var app = builder.Build();

app.MapGet("/orders", async (OrderDbContext orderDbContext)
    => await orderDbContext.Orders.ToListAsync())
    .Produces<List<Orderr>>(StatusCodes.Status201Created);

app.MapGet("/orders/{id:int}", async (OrderDbContext orderDbContext, int id)
    => await orderDbContext.Orders.FindAsync(id))
    .Produces<Orderr>(StatusCodes.Status201Created);

app.MapPost("/orders", async (OrderDbContext orderDbContext, Orderr order) =>
{
    await orderDbContext.Orders.AddAsync(order);
    await orderDbContext.SaveChangesAsync();
    return Results.Created($"/orders/{order.Id}", order);
}).Produces(StatusCodes.Status201Created);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
{
    services.AddDbContext<OrderDbContext>(
        opts =>
        {
            opts.UseNpgsql(configurationManager.GetConnectionString("OrderDb"));
        }, ServiceLifetime.Transient);
}