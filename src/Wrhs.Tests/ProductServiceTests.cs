
using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace Wrhs.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        [Test]
        public void CreateProductSaveToRepository()
        {
            var items = new List<Product>();
            var service = MakeProductService(items);
            var product = new Product
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "123456789012"
            };

            service.CreateProduct(product);

            Assert.AreEqual(1, items.Count);
            CollectionAssert.Contains(items, product);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void CantCreateProductWithEmptyCode(string code)
        {
            var items = new List<Product>();
            var service = MakeProductService(items);
            var product = new Product
            {
                Code = code,
                Name = "Product 1",
                EAN = "123456789012"
            };

            Assert.Throws<ArgumentException>(()=>{ service.CreateProduct(product); });
            CollectionAssert.IsEmpty(items);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void CantCreateProductWithEmptyName(string name)
        {
            var items = new List<Product>();
            var service = MakeProductService(items);
            var product = new Product
            {
                Code = "PROD1",
                Name = name,
                EAN = "123456789012"
            };

            Assert.Throws<ArgumentException>(()=>{ service.CreateProduct(product); });
            CollectionAssert.IsEmpty(items);
        }

        [Test]
        public void CantCreateProductWithExistingCode()
        {
            var items = new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789011" } };
            var service = MakeProductService(items);
            var product = new Product { Code="PROD1", Name="Product 2", EAN="123456789012" };

            Assert.Throws<ArgumentException>(()=> service.CreateProduct(product));
            Assert.AreEqual(1, items.Count);
        }

        [Test]
        public void CantCreateProductWithExistingEAN()
        {
            var items = new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789011" } };
            var service = MakeProductService(items);
            var product = new Product { Code="PROD2", Name="Product 2", EAN="123456789011" };

            Assert.Throws<ArgumentException>(()=> service.CreateProduct(product));
            Assert.AreEqual(1, items.Count);
        }

        protected Mock<IRepository<Product>> MakeProductRepo(List<Product> items)
        {
            var mock = new Mock<IRepository<Product>>();
            mock.Setup(m=>m.Save(It.IsAny<Product>()))
                .Callback((Product prod)=>{ items.Add(prod); });

            mock.Setup(m=>m.Get())
                .Returns(items);

            return mock;
        }

        protected ProductService MakeProductService(List<Product> items)
        {
            var repo = MakeProductRepo(items);
            return new ProductService(repo.Object);
        }
    }
}