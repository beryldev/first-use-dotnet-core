using Microsoft.EntityFrameworkCore;
using Wrhs.Common;
using Wrhs.Products;

namespace Wrhs.Data
{
    public class WrhsContext : DbContext
    {
        public WrhsContext() { }
        
        public WrhsContext(DbContextOptions<WrhsContext> options) : base(options)
        {
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentLine> DocumentLines { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(b => b.Name)
                .HasAnnotation("CaseSensitive", false);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./wrhs.db");
        }
    }
}