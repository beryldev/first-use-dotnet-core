
using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Products;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class CreateProductCmdValidatorTests
    {
        private readonly CreateProductCommandValidator validator;

        private readonly Mock<IProductService> productSrvMock;

        public CreateProductCmdValidatorTests()
        {
            productSrvMock = new Mock<IProductService>();
            productSrvMock.Setup(m=>m.CheckProductExistsByName(It.IsAny<string>())).Returns(false);
            productSrvMock.Setup(m=>m.CheckProductExistsByCode(It.IsAny<string>())).Returns(false);

            validator = new CreateProductCommandValidator(productSrvMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void ShouldReturnValidationErrorWhenEmptyName(string name)
        {
            var command = new CreateProductCommand { Name = name };

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("Name");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldReturnValidationErrorWhenEmptyCode(string code)
        {
            var command = new CreateProductCommand { Name = "Some name", Code = code };

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("Code");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenDuplicatedName()
        {
            productSrvMock.Setup(m=>m.CheckProductExistsByName(It.IsAny<string>())).Returns(true);
            var command = new CreateProductCommand { Name = "Some name", Code = "CODE" };

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("Name");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenDuplicatedCode()
        {
            productSrvMock.Setup(m=>m.CheckProductExistsByCode(It.IsAny<string>())).Returns(true);
            var command = new CreateProductCommand { Name = "Some name", Code = "CODE" };

            var result = validator.Validate(command);

            result.Should().NotBeNullOrEmpty();
            result.Select(x=>x.Field).Contains("Code");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenDuplicatedCodeAndName()
        {
            productSrvMock.Setup(m=>m.CheckProductExistsByCode(It.IsAny<string>())).Returns(true);
            productSrvMock.Setup(m=>m.CheckProductExistsByName(It.IsAny<string>())).Returns(true);
            var command = new CreateProductCommand { Name = "Some name", Code = "CODE" };

            var result = validator.Validate(command);

            result.Count().Should().Equals(2);
            result.Select(x=>x.Field).Contains("Code");
            result.Select(x=>x.Field).Contains("Name");
        }

        [Fact]
        public void ShouldReturnNoValidationErrosWhenPassValidCommand()
        {
            var command = new CreateProductCommand { Name = "Soma name", Code = "CODE"};

            var result = validator.Validate(command);

            result.Should().BeEmpty();
        }
    }
}