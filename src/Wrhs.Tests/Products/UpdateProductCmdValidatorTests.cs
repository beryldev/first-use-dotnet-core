using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Products;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class UpdateProductCmdValidatorTests
    {
        private readonly UpdateProductCommandValidator validator;

        private readonly Mock<IProductService> productSrvMock;

        public UpdateProductCmdValidatorTests()
        {
            productSrvMock = new Mock<IProductService>();
            productSrvMock.Setup(m=>m.CheckProductExists(It.IsAny<int>())).Returns(true);
            
            validator = new UpdateProductCommandValidator(productSrvMock.Object);
        }

        
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void ShouldReturnValidationErrorWhenEmptyName(string name)
        {
            var command = new UpdateProductCommand { Name = name };

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("Name");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void ShouldReturnValidationErrorWhenEmptyCode(string code)
        {
            var command = new UpdateProductCommand { Code = code };

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("Code");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenProductNotExists()
        {
            productSrvMock.Setup(m=>m.CheckProductExists(It.IsAny<int>())).Returns(false);
            var command = new UpdateProductCommand { Name = "name", Code = "code", ProductId = 1};

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("ProductId");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenChangeNameToExistedInOtherProduct()
        {
            productSrvMock.Setup(m=>m.GetProductByName("New name")).Returns(new Product{Id = 2, Name = "New name"});
            var command = new UpdateProductCommand 
            { 
                ProductId = 1,
                Name = "New name",
                Code = "NewCode"
            };

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("Name");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenChangeCodeToExistedInOtherProduct()
        {
            productSrvMock.Setup(m=>m.GetProductByCode("NewCode")).Returns(new Product{Id = 2, Code = "NewCode"});
            var command = new UpdateProductCommand 
            { 
                ProductId = 1,
                Name = "New name",
                Code = "NewCode"
            };

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("Code");
        }
    }
}