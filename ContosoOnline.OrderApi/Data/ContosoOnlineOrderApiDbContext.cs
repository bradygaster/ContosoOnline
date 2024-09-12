using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoOnline.OrderApi.DataModels;

namespace ContosoOnline.OrderApi.Data
{
    public class ContosoOnlineOrderApiDbContext : DbContext
    {
        public ContosoOnlineOrderApiDbContext (DbContextOptions<ContosoOnlineOrderApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContosoOnline.OrderApi.DataModels.Cart> Cart { get; set; } = default!;
        public DbSet<ContosoOnline.OrderApi.DataModels.CartItem> CartItem { get; set; } = default!;
        public DbSet<ContosoOnline.OrderApi.DataModels.Order> Order { get; set; } = default!;
    }
}
