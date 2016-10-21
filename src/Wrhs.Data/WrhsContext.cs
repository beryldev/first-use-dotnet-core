using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Wrhs.Data.ContextFactory;
using Wrhs.Operations;
using Wrhs.Products;

namespace Wrhs.Data
{
    public class WrhsContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Allocation> Allocations { get; set; }

        public WrhsContext(DbContextOptions<WrhsContext> options)
            : base(options){ }
    }

    public class WrhsContextFactory : IDbContextFactory<WrhsContext>
    {
        public WrhsContext Create(DbContextFactoryOptions options)
        {
            return SqliteContextFactory.Create("Filename=./wrhs.db");
        }
    }
}