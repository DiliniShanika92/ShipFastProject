using Microsoft.EntityFrameworkCore;
using ShipFast.Order.DataAccess.Models;

namespace ShipFast.Order.DataAccess
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext() { }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public virtual DbSet<Orderr> Orders { get; set; }
    }
}
