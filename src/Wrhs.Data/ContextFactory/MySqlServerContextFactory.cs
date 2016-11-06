using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace Wrhs.Data.ContextFactory
{
    public static class MySqlServerContextFactory
    {
        public static WrhsContext Create(string connectionstring)
        {
             var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<WrhsContext>();
            optionsBuilder.UseMySQL(connectionstring);

            return  new WrhsContext(optionsBuilder.Options);
        }
    }
}