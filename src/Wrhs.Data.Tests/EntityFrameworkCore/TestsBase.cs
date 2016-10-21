using Wrhs.Data.ContextFactory;

namespace Wrhs.Data.Tests.EntityFrameworkCore
{
    public class TestsBase
    {
        protected WrhsContext CreateContext()
        {
            //var context = InMemoryContextFactory.Create();
            var context = SqliteContextFactory.Create("Filename=./wrhs.db");
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }
}