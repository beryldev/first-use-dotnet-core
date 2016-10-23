using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Wrhs.Data.ContextFactory
{
    public static class SqlServerContextFactory
    {
        public static WrhsContext Create(string connectionstring)
        {
             var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<WrhsContext>();
            optionsBuilder.UseSqlServer(connectionstring);

            return  new WrhsContext(optionsBuilder.Options);
        }
    }
}