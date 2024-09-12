using Microsoft.EntityFrameworkCore;

namespace ContosoOnline.CatalogApi.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext (DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContosoOnline.CatalogApi.DataModels.Product> Products { get; set; } = default!;
    }
}
