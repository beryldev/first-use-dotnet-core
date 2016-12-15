using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests.Delivery
{
    public class ProcessDeliveryOperationCmdValidatorTests 
        : ProcessOperationCmdValidatorTestsBase<ProcessDeliveryOperationCommand>
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

        protected override ProcessDeliveryOperationCommand CreateCommand()
        {
            return new ProcessDeliveryOperationCommand
            {
                OperationGuid = "guid",
                DstLocation = "location",
                ProductId = 1,
                Quantity = 5
            };     
        }

        protected override IValidator<ProcessDeliveryOperationCommand> CreateValidator(IOperationService operationSrv, IProductService productSrv)
        {
            return new ProcessDeliveryOperationCommandValidator(operationSrvMock.Object, productSrvMock.Object);
        }

        protected override OperationType GetInvalidOperationType()
        {
            return OperationType.Relocation;
        }

        protected override OperationType GetValidOperationType()
        {
            return OperationType.Delivery;
        }
    }
}