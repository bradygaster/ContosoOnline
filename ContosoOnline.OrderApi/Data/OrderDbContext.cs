using Microsoft.EntityFrameworkCore;

namespace ContosoOnline.OrderApi.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext (DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContosoOnline.OrderApi.DataModels.Cart> Cart { get; set; } = default!;
        public DbSet<ContosoOnline.OrderApi.DataModels.CartItem> CartItem { get; set; } = default!;
        public DbSet<ContosoOnline.OrderApi.DataModels.Order> Order { get; set; } = default!;
    }
}
