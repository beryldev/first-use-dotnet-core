using System;
using System.Linq;
using NUnit.Framework;
using Wrhs.Data;
using Wrhs.Products;

namespace Wrhs.Tests.Data
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        [Test]
        public void CanSaveProductToRepo()
        {
            var repo = new ProductRepository();
            var prod = new Product
            {
                Code = "PROD5",
                Name = "Product 5",
                EAN = "555555",
                SKU = "555",
                Description = "some 5 desc"
            };

            repo.Save(prod);

            Assert.NotNull(prod.Id);
            Assert.Greater(repo.Get().Count(), 0);
        }
    }
}