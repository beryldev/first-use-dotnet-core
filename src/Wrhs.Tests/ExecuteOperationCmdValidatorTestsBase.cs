using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class ExecuteOperationCmdValidatorTestsBase<TCommand>
        : CommandValidatorTestsBase<TCommand> where TCommand : ExecuteOperationCommand
    {
        protected readonly Mock<IOperationService> operationSrvMock;
        protected readonly TCommand command;

        public ExecuteOperationCmdValidatorTestsBase()
        {       
            operationSrvMock = new Mock<IOperationService>();
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>())).Returns(true);
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsAny<string>()))
                .Returns(new Operation { Type = GetValidOperationType()});

            command = CreateCommand();

            validator = CreateValidator(operationSrvMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenInvalidOperationGuid(string guid)
        {
            command.OperationGuid = guid;

            var results = validator.Validate(command);

            AssertSingleError(results, "OperationGuid");
        }

        [Fact]
        public void ShouldReturnErrorWhenOperationNotFound()
        {
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>())).Returns(false);
            
            var results = validator.Validate(command);

            AssertSingleError(results, "OperationGuid");
        }

        [Fact]
        public void ShouldReturnErrorWhenInvalidOperationType()
        {
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsAny<string>()))
                .Returns(new Operation { Type = GetInvalidOperationType()});

            var results = validator.Validate(command);

            AssertSingleError(results, "OperationGuid");
        }

        protected abstract TCommand CreateCommand();

        protected abstract IValidator<TCommand> CreateValidator(IOperationService operationSrv);

        protected abstract OperationType GetValidOperationType();

        protected abstract OperationType GetInvalidOperationType();

    }
}