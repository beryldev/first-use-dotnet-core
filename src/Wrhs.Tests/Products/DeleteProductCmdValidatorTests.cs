using System.Linq;
using FluentAssertions;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class DeleteProductCmdValidatorTests
    {
        private readonly DeleteProductCommandValidator validator;

        public DeleteProductCmdValidatorTests()
        {
            validator = new DeleteProductCommandValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]        
        public void ShouldReturnErrorsWhenInvalidProductId(int productId)
        {
            var command = new DeleteProductCommand { ProductId = productId};

            var result = validator.Validate(command);

            result.Should().HaveCount(1);
            result.Select(x=>x.Field).Should().Contain("ProductId");
        }

    }
}