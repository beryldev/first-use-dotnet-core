using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Wrhs.Data.ContextFactory
{
    public static class SqliteContextFactory
    {
        public static WrhsContext Create(string connectionstring)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<WrhsContext>();
            optionsBuilder.UseSqlite(connectionstring);
            
            return  new WrhsContext(optionsBuilder.Options);
        }
    }
}