using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class ProcessOperationCmdValidatorTestsBase<TCommand>
        : CommandValidatorTestsBase<TCommand>
        where TCommand : ProcessOperationCommand
    {
        protected readonly TCommand command;
        protected readonly Mock<IOperationService> operationSrvMock;
        protected readonly Mock<IProductService> productSrvMock;
    
        protected ProcessOperationCmdValidatorTestsBase()
        {
            operationSrvMock = new Mock<IOperationService>();
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>())).Returns(true);
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsAny<string>()))
                .Returns(new Operation{Type = GetValidOperationType()});

            productSrvMock = new Mock<IProductService>();
            productSrvMock.Setup(m=>m.CheckProductExists(It.IsAny<int>())).Returns(true);

            validator = CreateValidator(operationSrvMock.Object, productSrvMock.Object);
            command = CreateCommand();
        }

        protected abstract TCommand CreateCommand();

        protected abstract IValidator<TCommand> CreateValidator(IOperationService operationSrv, IProductService productSrv);
    
        protected abstract OperationType GetValidOperationType();

        protected abstract OperationType GetInvalidOperationType();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenIvalidOperationGuid(string guid)
        {
            command.OperationGuid = guid;

            var results = Act(command);

            AssertSingleError(results, "OperationGuid");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShouldReturnErrorWhenInvalidProductId(int productId)
        {
            command.ProductId = productId;

            var results = Act(command);

            AssertSingleError(results, "ProductId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShouldReturnErrorWhenInvalidQuantity(decimal quantity)
        {
            command.Quantity = quantity;

            var results = Act(command);

            AssertSingleError(results, "Quantity");
        }

        [Fact]
        public void ShouldReturnErrorWhenOperationNotExists()
        {
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>())).Returns(false);
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsAny<string>())).Returns(null as Operation);

            var results = Act(command);

            AssertSingleError(results, "OperationGuid");
        }

        [Fact]
        public void ShouldReturnErrorWhenInvalidOperationType()
        {
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsAny<string>()))
                .Returns(new Operation{Type=GetInvalidOperationType()});

            var results = Act(command);

            AssertSingleError(results, "OperationGuid");
        }

        [Fact]
        public void ShouldReturnErrorWhenProductNotFound()
        {
            productSrvMock.Setup(m=>m.CheckProductExists(It.IsAny<int>())).Returns(false);
            var results = Act(command);

            AssertSingleError(results, "ProductId");
        }
    }
}