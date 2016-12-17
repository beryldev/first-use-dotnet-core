using System;
using System.Collections.Generic;
using FluentAssertions;
using Wrhs.Data.Service;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Service
{
    public class ProductServiceTests : ServiceTestsBase<Product>, IDisposable
    {
        private readonly ProductService service;

        public ProductServiceTests() : base()
        {
            service = new ProductService(context);
        }

        protected override BaseService<Product> GetService()
        {
            return service as BaseService<Product>;
        }

        [Fact]
        public void ShouldTrueWhenProdutcExistsById()
        {
            var result = service.CheckProductExists(2);

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseWhenProductNotExistsById()
        {
            var result = service.CheckProductExists(6);

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueWhenProdExistsByCode()
        {
            var result = service.CheckProductExistsByCode("PROD1");

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseWhenProdNotExistsByCode()
        {
            var result = service.CheckProductExistsByCode("PROD11");

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueWhenProdExistsByName()
        {
            var result = service.CheckProductExistsByName("Product 1");

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseWhenProdNotExistsByName()
        {
            var result = service.CheckProductExistsByName("Product 11");

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnProductOnGetByCodeWhenExists()
        {
            var result = service.GetProductByCode("PROD1");

            result.Should().BeOfType<Product>();
            result.Name.Should().Be("Product 1");
            result.Code.Should().Be("PROD1");
        }

        [Fact]
        public void ShouldReturnNullOnGetByCodeWhenNotExists()
        {
            var result = service.GetProductByCode("PROD11");

            result.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnProductOnGetByIdWhenExists()
        {
            var result = service.GetProductById(1);

            result.Should().BeOfType<Product>();
            result.Name.Should().Be("Product 1");
            result.Code.Should().Be("PROD1");
        }

        [Fact]
        public void ShouldReturnNullOnGetByIdWhenNotExists()
        {
            var result = service.GetProductById(11);

            result.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnProductOnGetByNameWhenExists()
        {
            var result = service.GetProductByName("Product 1");

            result.Should().BeOfType<Product>();
            result.Name.Should().Be("Product 1");
            result.Code.Should().Be("PROD1");
        }

        [Fact]
        public void ShouldReturnNullOnGetByNameWhenNotExists()
        {
            var result = service.GetProductByName("Product 11");

            result.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnFilteredProducts()
        {
            context.Products.Add(new Product { Name="Product 1", Code= "PROD1"});
            context.Products.Add(new Product { Name="Product 9", Code= "PROD9"});
            context.Products.Add(new Product { Name="Product 91", Code= "PROD91"});
            context.Products.Add(new Product { Name="Product 82", Code= "PROD82"});
            context.SaveChanges();

            var filter = new Dictionary<string, object>();
            filter.Add("Name", "Product 9");

            var result = service.FilterProducts(filter);

            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldReturnCombinedFilteredProducts()
        {
            context.Products.Add(new Product { Name="Product 7", Code= "PROD7"});
            context.Products.Add(new Product { Name="Product 21", Code= "PROD21"});
            context.Products.Add(new Product { Name="Product 31", Code= "PROD31"});
            context.Products.Add(new Product { Name="Product 32", Code= "PROD32"});
            context.SaveChanges();

            context.Products.Add(new Product { Name="Product 7", Code="XPROD1"});
            context.SaveChanges();
            var filter = new Dictionary<string, object>();
            filter.Add("Name", "Product 7");
            filter.Add("Code", "XPROD1");

            var result = service.FilterProducts(filter);

            result.Items.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(1, 2, 2)]
        public void ShouldReturnRequestedPageAndPageSizeOnFilter(int page, int pageSize, int expected)
        {
            context.Products.Add(new Product { Name="Product 7", Code= "PROD7"});
            context.Products.Add(new Product { Name="Product 21", Code= "PROD21"});
            context.Products.Add(new Product { Name="Product 31", Code= "PROD31"});
            context.Products.Add(new Product { Name="Product 32", Code= "PROD32"});
            context.SaveChanges();

            var filter = new Dictionary<string, object>();
            filter.Add("Name", "Product 3");

            var result = service.FilterProducts(filter, page, pageSize);

            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.Items.Should().HaveCount(expected);
        }

        protected override Product CreateEntity(int i)
        {
            return new Product { Name = $"Product {i+1}", Code = $"PROD{i+1}" };
        }
    }
}