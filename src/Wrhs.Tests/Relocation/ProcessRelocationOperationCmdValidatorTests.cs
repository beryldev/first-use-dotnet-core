using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests.Relocation
{
    public class ProcessRelocationOperationCmdValidatorTests
        : ProcessOperationCmdValidatorTestsBase<ProcessRelocationOperationCommand>
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenInvalidDstLocation(string dstLocation)
        {
            command.DstLocation = dstLocation;

            var results = Act(command);

            AssertSingleError(results, "DstLocation");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenInvalidSrcLocation(string srcLocation)
        {
            command.SrcLocation = srcLocation;

            var results = Act(command);

            AssertSingleError(results, "SrcLocation");
        }

        [Theory]
        [InlineData("loc")]
        [InlineData("Loc")]
        [InlineData("LoC")]
        [InlineData("LOC")]
        [InlineData("lOc")]
        public void ShouldReturnErrorWhenSoruceIsSameDestination(string location)
        {
            command.SrcLocation = location;
            command.DstLocation = location;

            var results = Act(command);

            AssertSingleError(results, "SrcLocation | DstLocation");
        }
        
        protected override ProcessRelocationOperationCommand CreateCommand()
        {
            return new ProcessRelocationOperationCommand
            {
                OperationGuid = "guid",
                SrcLocation = "srcLoc",
                DstLocation = "dstLoc",
                ProductId = 1,
                Quantity = 5
            };
        }

        protected override IValidator<ProcessRelocationOperationCommand> CreateValidator(IOperationService operationSrv, IProductService productSrv)
        {
            return new ProcessRelocationOperationCommandValidator(operationSrvMock.Object, productSrvMock.Object);
        }

        protected override OperationType GetInvalidOperationType()
        {
            return OperationType.Delivery;
        }

        protected override OperationType GetValidOperationType()
        {
            return OperationType.Relocation;
        }
    }
}