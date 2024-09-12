using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoOnline.CatalogApi.DataModels;

namespace ContosoOnline.CatalogApi.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext (DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContosoOnline.CatalogApi.DataModels.Product> Product { get; set; } = default!;
    }
}
