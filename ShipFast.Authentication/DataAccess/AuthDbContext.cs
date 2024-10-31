using Microsoft.EntityFrameworkCore;
using ShipFast.Authentication.DataAccess.Models;

namespace ShipFast.Authentication.DataAccess
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext() { }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
    }
}
