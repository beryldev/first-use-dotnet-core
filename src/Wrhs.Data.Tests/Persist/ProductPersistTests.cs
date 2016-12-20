using System;
using System.Linq;
using FluentAssertions;
using Wrhs.Data.Persist;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Persist
{
    public class ProductPersistTests : TestsBase, IDisposable
    {
        private readonly ProductPersist productPersist;

        public ProductPersistTests()
        {
            productPersist = new ProductPersist(context);
        }

        [Fact]
        public void ShouldStoreInContextWhenSave()
        {
            var product = new Product{Name = "name", Code = "code"};

            productPersist.Save(product);

            context.Products.Count().Should().Equals(1);
        }

        [Fact]
        public void ShouldReplaceProductInCotextWhenUpdate()
        {
            var product = new Product { Name = "Name", Code = "Code"};
            context.Products.Add(product);
            context.SaveChanges();

            product.Name = "NewName";
            productPersist.Update(product);

            context.Products.Where(p=>p.Name == "NewName").Count().Should().Equals(1);
        }

        [Fact]
        public void ShouldRemoveProductFromContextOnDelete()
        {
            var product = new Product { Name = "Name", Code = "Code"};
            context.Products.Add(product);
            context.SaveChanges();

            productPersist.Delete(product);

            context.Products.Should().BeEmpty();
        }

        [Fact]
        public void ShouldNotFailWhenTryDeleteNull()
        {
            var product = new Product { Name = "Name", Code = "Code"};
            context.Products.Add(product);
            context.SaveChanges();

            productPersist.Delete(null);

            context.Products.Should().NotBeNullOrEmpty();
        }
    }
}