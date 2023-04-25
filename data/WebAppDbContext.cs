using Microsoft.EntityFrameworkCore;
using server.model;

namespace server.data
{
    public class WebAppDbContext : DbContext
    {
        public WebAppDbContext(DbContextOptions<WebAppDbContext> options):base(options)
        {

        }

        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<OrderModel> Orders => Set<OrderModel>();
    }
}