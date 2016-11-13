using System.Linq;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class UpdateProductCommandValidatorTests : ProductCommandTestsBase
    {
        [Fact]
        public void ReturnValidationFailMessageWhenProductDoesNotExists()
        {
            var repo = MakeProductRepository(MakeProductList());
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 3,
                Code = "PROD2",
                Name = "Product"
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("ProductId", result.First().Field);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ReturnValidationFailMessageWhenNewCodeIsEmpty(string code)
        {
            var repo = MakeProductRepository(MakeProductList());
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = code,
                Name = "Product"
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("Code", result.First().Field);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ReturnValidationFailMessageWhenNewNameIsEmpty(string name)
        {
            var repo = MakeProductRepository( MakeProductList());
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD1",
                Name = name
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("Name", result.First().Field);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ReturnValidationFailMessagesWhenNewCodeAndNameAreEmpty(string value)
        {
            var repo = MakeProductRepository(MakeProductList());
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = value,
                Name = value
            };

            var result = validator.Validate(command);

            Assert.Equal(2, result.Count());
            Assert.Contains("Code", result.Select(item=>item.Field));
            Assert.Contains("Name", result.Select(item=>item.Field));
        }

        [Fact]
        public void ReturnValidationFailMessageWhenNewCodeDuplicated()
        {
            var repo = MakeProductRepository(MakeProductList());
            repo.Save(new Product{Id = 2, Code = "PROD2", Name="Product 2", EAN = "111111111" });
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD2",
                Name = "Product 1",
                EAN = "111111111111"
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("Code", result.First().Field);
        }

        [Fact]
        public void ReturnNoValidationFailMessageWhenCodeAreNotChanged()
        {
            var repo = MakeProductRepository(MakeProductList());
            repo.Save(new Product{Id = 2, Code = "PROD2", Name="Product 2", EAN = "111111111" });
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD1",
                Name = "Product 1 new",
                EAN = "111111111111"
            };

            var result = validator.Validate(command);

            Assert.Empty(result);
        }

        [Fact]
        public void ReturnValidationFailMessageWhenNewEANDuplicated()
        {
            var repo = MakeProductRepository(MakeProductList());
            repo.Save(new Product{Id = 2, Code = "PROD2", Name="Product 2", EAN = "111111111111" });
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD1",
                Name = "Product 1",
                EAN = "111111111111"
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("EAN", result.First().Field);
        }

        [Fact]
        public void ReturnNoValidationFailMessageWhenEANAreNotChanged()
        {
            var repo = MakeProductRepository(MakeProductList());
            repo.Save(new Product{Id = 2, Code = "PROD2", Name="Product 2", EAN = "111111111" });
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD1",
                Name = "Product 1 new",
                EAN = "111111111111"
            };

            var result = validator.Validate(command);

            Assert.Empty(result);
        }

    }
}