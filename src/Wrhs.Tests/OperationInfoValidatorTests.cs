using Moq;
using Wrhs.Common;
using Xunit;

namespace Wrhs.Tests
{
    public class OperationInfoValidatorTests : ValidatorTestsBase<IValidableOperationInfo>
    {
        private readonly Mock<IOperationService> operationSrvMock;

        public OperationInfoValidatorTests ()
        {
            operationSrvMock = new Mock<IOperationService>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenInvalidOperationGuid(string guid)
        {
            var info = new OperationInfo{ OperationGuid = guid };
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>())).Returns(true);
            validator = new OperationInfoValidator(operationSrvMock.Object);

            var results = Act(info);

            AssertSingleError(results, "OperationGuid");
        }

        [Fact]
        public void ShouldReturnErrorWhenBeginOperationAndExistsOperationWithPassedGuid()
        {
            var info = new OperationInfo { OperationGuid = "some-guid" };
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(info.OperationGuid))
                .Returns(true);
            validator = new OperationInfoValidator(operationSrvMock.Object, true);

            var results = Act(info);
     
            AssertSingleError(results, "OperationGuid");
        }

        [Fact]
        public void ShouldReturnErrorWhenNotBeginOperationAndOperationNotExists()
        {
            var info = new OperationInfo { OperationGuid = "some-guid" };
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>())).Returns(false);
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsAny<string>())).Returns(null as Operation);
            validator = new OperationInfoValidator(operationSrvMock.Object, false);

            var results = Act(info);

            AssertSingleError(results, "OperationGuid");
        }


        class OperationInfo : IValidableOperationInfo
        {
            public string OperationGuid
            {
               get; set; 
            }
        }
    }
}