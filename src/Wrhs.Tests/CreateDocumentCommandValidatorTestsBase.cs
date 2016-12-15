using System.Collections.Generic;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class CreateDocumentCommandValidatorTestsBase<T> : CommandValidatorTestsBase<T>
    where T : CreateDocumentCommand
    {
        protected readonly Mock<IProductService> productSrvMock;

        public CreateDocumentCommandValidatorTestsBase()
        {
            productSrvMock = new Mock<IProductService>();
            productSrvMock.Setup(m=>m.CheckProductExists(It.IsAny<int>()))
                .Returns(true);
            validator = CreateValidator(productSrvMock.Object);
        }

        protected abstract IValidator<T> CreateValidator(IProductService productSrv);

        protected abstract T CreateCommand();
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]
        public void ShouldReturnValidationErrorWhenInvalidProductId(int productId)
        {
            var command = CreateCommand();
            command.Lines = new List<CreateDocumentCommand.DocumentLine> 
                { new CreateDocumentCommand.DocumentLine { ProductId=productId, Quantity=2, SrcLocation="s", DstLocation="d" } };
            
            var result = Act(command);

            AssertSingleError(result, "ProductId");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenProductNotExits()
        {
            productSrvMock.Setup(m=>m.CheckProductExists(It.IsAny<int>()))
                .Returns(false);
            var command = CreateCommand();
            command.Lines = new List<CreateDocumentCommand.DocumentLine> 
                { new CreateDocumentCommand.DocumentLine { ProductId = 1, Quantity=2, SrcLocation="s", DstLocation="d" } };

            var result = Act(command);

            AssertSingleError(result, "ProductId");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]
        public void ShouldReturnValidationErrorWhenInvalidQuantity(decimal quantity)
        {        
            var command = CreateCommand();
            command.Lines = new List<CreateDocumentCommand.DocumentLine> 
                { new CreateDocumentCommand.DocumentLine { ProductId = 1 , Quantity = quantity, SrcLocation="s", DstLocation="d"}};

            var result = Act(command);

            AssertSingleError(result, "Quantity");
        }
    }
}