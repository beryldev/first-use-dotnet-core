using System;
using Wrhs.Data.ContextFactory;

namespace Wrhs.Data.Tests
{
    public abstract class TestsBase : IDisposable
    {
        protected readonly WrhsContext context;

        protected TestsBase()
        {
            context = InMemoryContextFactory.Create();
            //context = SqlServerContextFactory.Create("Server=192.168.5.15;Database=wrhs;User Id=SA;Password=Password123;");
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
        }
    }
}