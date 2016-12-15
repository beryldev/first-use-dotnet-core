using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests
{
    public class ProductInfoValidatorTests
    {
        private readonly ProductInfoValidator validator;
        private readonly Mock<IProductService> productSrvMock;
        private readonly Mock<IValidableProductInfo> infoMock;

        public ProductInfoValidatorTests()
        {
            productSrvMock = new Mock<IProductService>();
            productSrvMock.Setup(m=>m.CheckProductExists(It.IsAny<int>()))
                .Returns(true);
            validator = new ProductInfoValidator(productSrvMock.Object);
            infoMock = new Mock<IValidableProductInfo>();
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]
        public void ShouldReturnValidationErrorWhenInvalidProductId(int productId)
        {
            var info = infoMock.Object;
            info.ProductId = productId;
            
            var result = validator.Validate(info);

            result.Should().NotBeEmpty();
            result.Select(x=>x.Field).Should().Contain("ProductId");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenProductNotExits()
        {
            var info = infoMock.Object;
            info.ProductId = 1;
            productSrvMock.Setup(m=>m.CheckProductExists(It.IsAny<int>()))
                .Returns(false);

            var result = validator.Validate(info);

            result.Should().NotBeEmpty();
            result.Select(x=>x.Field).Should().Contain("ProductId");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]
        public void ShouldReturnValidationErrorWhenInvalidQuantity(decimal quantity)
        {   
            var info = infoMock.Object;
            info.ProductId = 1;
            info.Quantity = quantity;

            var result = validator.Validate(info);

            result.Should().NotBeEmpty();
            result.Select(x=>x.Field).Should().Contain("Quantity");
        }
    }
}