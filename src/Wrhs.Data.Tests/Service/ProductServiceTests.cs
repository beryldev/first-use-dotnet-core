using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Wrhs.Data.Service;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Service
{
    public class ProductServiceTests : ServiceTestsBase<Product>, IDisposable
    {
        public ProductServiceTests() : base()
        {
            
        }

        [Fact]
        public void ShouldTrueWhenProdutcExistsById()
        {
            var result = (service as ProductService).CheckProductExists(2);

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseWhenProductNotExistsById()
        {
            var result = (service as ProductService).CheckProductExists(6);

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueWhenProdExistsByCode()
        {
            var result = (service as ProductService).CheckProductExistsByCode("PROD1");

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseWhenProdNotExistsByCode()
        {
            var result = (service as ProductService).CheckProductExistsByCode("PROD11");

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueWhenProdExistsByName()
        {
            var result = (service as ProductService).CheckProductExistsByName("Product 1");

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseWhenProdNotExistsByName()
        {
            var result = (service as ProductService).CheckProductExistsByName("Product 11");

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnProductOnGetByCodeWhenExists()
        {
            var result = (service as ProductService).GetProductByCode("PROD1");

            result.Should().BeOfType<Product>();
            result.Name.Should().Be("Product 1");
            result.Code.Should().Be("PROD1");
        }

        [Fact]
        public void ShouldReturnNullOnGetByCodeWhenNotExists()
        {
            var result = (service as ProductService).GetProductByCode("PROD11");

            result.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnProductOnGetByIdWhenExists()
        {
            var result = (service as ProductService).GetProductById(1);

            result.Should().BeOfType<Product>();
            result.Name.Should().Be("Product 1");
            result.Code.Should().Be("PROD1");
        }

        [Fact]
        public void ShouldReturnNullOnGetByIdWhenNotExists()
        {
            var result = (service as ProductService).GetProductById(11);

            result.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnProductOnGetByNameWhenExists()
        {
            var result = (service as ProductService).GetProductByName("Product 1");

            result.Should().BeOfType<Product>();
            result.Name.Should().Be("Product 1");
            result.Code.Should().Be("PROD1");
        }

        [Fact]
        public void ShouldReturnNullOnGetByNameWhenNotExists()
        {
            var result = (service as ProductService).GetProductByName("Product 11");

            result.Should().BeNull();
        }

        // [Fact]
        // public void ShouldReturnPageWithProductsOnGet()
        // {
        //     var result = service.Get();

        //     result.Should().NotBeNull();
        //     result.Items.Should().NotBeNullOrEmpty();
        // }

        // [Fact]
        // public void ShouldReturnDefaultFirstPageOnGetWithoutParameters()
        // {
        //     var result = service.Get();

        //     result.Page.Should().Be(1);
        //     result.PageSize.Should().Be(20);
        // }

        // [Fact]
        // public void ShouldReturnRequestedPageWithDefaultPageSizeOnGet()
        // {
        //     var result = service.Get(2);

        //     result.Page.Should().Be(2);
        //     result.PageSize.Should().Be(20);
        // }

        // [Fact]
        // public void ShouldReturnRequestedPageAndPageSizeOnGet()
        // {
        //     var result = service.Get(4, 10);

        //     result.Page.Should().Be(4);
        //     result.PageSize.Should().Be(10);
        // }

        // [Theory]
        // [InlineData(1, 2, 2)]
        // [InlineData(2, 2, 2)]
        // [InlineData(1, 4, 4)]
        // [InlineData(2, 3, 1)]        
        // public void ShouldReturnRequestedAndLimitedData(int page, int pageSize, int expected)
        // {
        //     var result = service.Get(page, pageSize);

        //     result.Items.Should().HaveCount(expected);
        // }

        // [Theory]
        // [InlineData(1, 4)]
        // [InlineData(2, 0)]
        // [InlineData(0, 4)]
        // public void ShouldReturnRequestedDataWithDefaultPageSize(int page, int expected)
        // {
        //     var result = service.Get(page);

        //     result.Items.Should().HaveCount(expected);
        // }

        // [Fact]
        // public void ShouldReturnRequestedDataWithDefaultPageAndPageSize()
        // {
        //     var result = service.Get();

        //     result.Items.Should().HaveCount(4);
        // }

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

            var result = (service as ProductService).FilterProducts(filter);

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

            var result = (service as ProductService).FilterProducts(filter);

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

            var result = (service as ProductService).FilterProducts(filter, page, pageSize);

            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.Items.Should().HaveCount(expected);
        }

        protected override BaseService<Product> CreateService(WrhsContext context)
        {
            return new ProductService(context);
        }

        protected override Product CreateEntity(int i)
        {
            return new Product { Name = $"Product {i+1}", Code = $"PROD{i+1}" };
        }
    }
}