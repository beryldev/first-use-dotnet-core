using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class CreateProductCommandValidatorTests : ProductCommandTestsBase
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ReturnOneValidationFailMessageWhenOnlyProductCodeEmpty(string code)
        {
            var repo = MakeProductRepository();
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = code,
                Name = "Product 1",
                EAN = "123456789011"
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("code", result.First().Field, true);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ReturnOneValidationFailMessageWhenOnlyProductNameEmpty(string name)
        {
            var repo = MakeProductRepository();
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = name,
                EAN = "123456789011"
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("name", result.First().Field, true);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ReturnTwoValidationFailMessagesWhenProductCodeAndNameEmpty(string value)
        {
            var repo = MakeProductRepository();
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = value,
                Name = value,
                EAN = "123456789011"
            };

            var result = validator.Validate(command);

            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData("PROD1", "PROD1")]
        [InlineData("prod1", "PROD1")]
        [InlineData("PROD1", "prod1")]
        [InlineData("prod1", "prod1")]
        [InlineData("pRoD1", "PrOd1")]
        public void ReturnValidationFailMessageWhenOnlyProducCodeDuplicated(string presentCode, string newCode)
        {
            var repo = MakeProductRepository(new List<Product>{ new Product { Code=presentCode.ToUpper(), Name="Product 1", EAN="123456789012" } });
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = newCode,
                Name = "Product 1",
                EAN = "123456789011"
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("code", result.First().Field, true);
        }

        [Fact]
        public void ReturnValidationFailMessageWhenOnlyProducEANDuplicated()
        {
            var repo = MakeProductRepository(new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789012" } });
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = "PROD2",
                Name = "Product 2",
                EAN = "123456789012"
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("EAN", result.First().Field, true);
        }

        [Fact]
        [InlineData("PROD1", "PROD1")]
        [InlineData("prod1", "PROD1")]
        [InlineData("PROD1", "prod1")]
        [InlineData("prod1", "prod1")]
        [InlineData("pRoD1", "PrOd1")]
        public void ReturnValidationFailMessagesWhenEANAndCodeDuplicated(string presentCode, string newCode)
        {
            var repo = MakeProductRepository(new List<Product>{ new Product { Code=presentCode.ToUpper(), Name="Product 1", EAN="123456789012" } });
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = newCode,
                Name = "Product 2",
                EAN = "123456789012"
            };

            var result = validator.Validate(command);

            Assert.Equal(2, result.Count());
        }

        protected IRepository<Product> MakeProductRepository()
        {
            return MakeProductRepository(new List<Product>());
        }
    }
}