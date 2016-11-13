using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Wrhs.Data.ContextFactory
{
    public static class InMemoryContextFactory
    {
        public static WrhsContext Create()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<WrhsContext>();

            optionsBuilder.UseInMemoryDatabase()
                .UseInternalServiceProvider(serviceProvider);
            
            return  new WrhsContext(optionsBuilder.Options);
        }
    }
}