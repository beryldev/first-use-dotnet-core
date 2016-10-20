using Microsoft.EntityFrameworkCore;
using Wrhs.Products;

namespace Wrhs.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./wrhs.db");
        }
    }
}