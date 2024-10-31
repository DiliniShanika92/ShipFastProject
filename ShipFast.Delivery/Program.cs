using Microsoft.EntityFrameworkCore;
using ShipFast.Delivery.DataAccess;
using ShipFast.Delivery.DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(
    builder.Services,
    builder.Configuration
);

var app = builder.Build();

app.MapGet("/deliveries", async(DeliveryDbContext deliveryDbContext) 
    => await deliveryDbContext.Deliveries.ToListAsync())
    .Produces<List<Deliveryy>>(StatusCodes.Status201Created);

app.MapGet("/deliveries/{id:int}", async (DeliveryDbContext deliveryDbContext, int id)
    => await deliveryDbContext.Deliveries.FindAsync(id))
    .Produces<Deliveryy>(StatusCodes.Status201Created);

app.MapPost("/deliveries", async (DeliveryDbContext deliveryDbContext, Deliveryy delivery) =>
{
    await deliveryDbContext.Deliveries.AddAsync(delivery);
    await deliveryDbContext.SaveChangesAsync();
    return Results.Created($"/deliveries/{delivery.Id}", delivery);
}).Produces(StatusCodes.Status201Created);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
{
    services.AddDbContext<DeliveryDbContext>(
        opts =>
        {
            opts.UseNpgsql(configurationManager.GetConnectionString("DeliveryDb"));
        }, ServiceLifetime.Transient);
}