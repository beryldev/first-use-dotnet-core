using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Wrhs.Data.ContextFactory;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;
using Wrhs.Operations.Release;
using Wrhs.Operations.Relocation;
using Wrhs.Products;

namespace Wrhs.Data
{
    public class WrhsContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Allocation> Allocations { get; set; }

        public DbSet<DeliveryDocument> DeliveryDocuments { get; set; }

        public DbSet<DeliveryDocumentLine> DeliveryDocumentLines { get; set; }

        public DbSet<RelocationDocument> RelocationDocuments { get; set; }

        public DbSet<RelocationDocumentLine> RelocationDocumentLines { get; set; }

        public DbSet<ReleaseDocument> ReleaseDocuments { get; set; }

        public DbSet<ReleaseDocumentLine> ReleaseDocumentLines { get; set; }

        public DbSet<Stock> StocksCache { get; set; }

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