using System.Collections.Generic;
using System.Linq;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class DeleteProductCommandValidatorTests : ProductCommandTestsBase
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-103)]
        [InlineData(-82)]
        public void ReturnValidationFailMessageWhenPasedProductIdIsNegative(int id)
        {
            var repo = MakeProductRepository(new List<Product>());
            var validator = new DeleteProductCommandValidator(repo);
            var command = new DeleteProductCommand
            {
                ProductId = id
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("ProductId", result.First().Field);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-132)]
        [InlineData(-83)]
        public void ReturnValidationFailMessageWhenProductDoesNotExists(int id)
        {
            var repo = MakeProductRepository(new List<Product>());
            var validator = new DeleteProductCommandValidator(repo);
            var command = new DeleteProductCommand
            {
                ProductId = id
            };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("ProductId", result.First().Field);
        }

        [Fact]
        public void ReturnNoValidationFailMessageWhenCommandIsValid()
        {
            var repo = MakeProductRepository(MakeProductList());
            var validator = new DeleteProductCommandValidator(repo);
            var command = new DeleteProductCommand
            {
                ProductId = repo.Get().First().Id
            };

            var result = validator.Validate(command);

            Assert.Empty(result);
        }
    }
}