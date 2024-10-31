using Microsoft.EntityFrameworkCore;
using ShipFast.Delivery.DataAccess.Models;

namespace ShipFast.Delivery.DataAccess
{
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext() { }

        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options) { }

        public virtual DbSet<Deliveryy> Deliveries { get; set; }
    }
}
