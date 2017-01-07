using System.Collections.Generic;
using Moq;
using Wrhs.Common;
using Xunit;

namespace Wrhs.Tests
{
    public class ExecuteOperationCmdValidatorTests
        : CommandValidatorTestsBase<ExecuteOperationCommand>
    {
        protected readonly Mock<IOperationService> operationSrvMock;
        protected readonly ExecuteOperationCommand command;

        public ExecuteOperationCmdValidatorTests()
        {       
            operationSrvMock = new Mock<IOperationService>();
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>())).Returns(true);
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsAny<string>()))
                .Returns(new Operation{Shifts = new List<Shift>{
                    new Shift { ProductId=1, Quantity=10, Location="loc1"}
                }});

            command = CreateCommand();

            validator = new ExecuteOperationCommandValidator(operationSrvMock.Object);
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
        public void ShouldReturnErrorWhenOperationNotProcessedTotaly()
        {

            var results = validator.Validate(command);

            AssertSingleError(results, "OperationGuid");
        }

        protected ExecuteOperationCommand CreateCommand()
        {
            return new ExecuteOperationCommand { OperationGuid = "some-guid"} ;
        }

    }
}