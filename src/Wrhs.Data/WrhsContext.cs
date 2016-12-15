using Microsoft.EntityFrameworkCore;
using Wrhs.Common;
using Wrhs.Products;

namespace Wrhs.Data
{
    public class WrhsContext : DbContext
    {
        public WrhsContext() { }
        
        public WrhsContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentLine> DocumentLines { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=192.168.5.15;Database=wrhs;User Id=SA;Password=Password123;");
        }
    }
}