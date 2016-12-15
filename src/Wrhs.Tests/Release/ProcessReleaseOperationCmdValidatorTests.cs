using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;
using Wrhs.Services;

namespace Wrhs.Tests.Release
{
    public class ProcessReleaseOperationCmdValidatorTests
        : ProcessOperationCmdValidatorTestsBase<ProcessReleaseOperationCommand>
    {
        protected override ProcessReleaseOperationCommand CreateCommand()
        {
            return new ProcessReleaseOperationCommand
            {
                OperationGuid = "guid",
                SrcLocation = "srcLoc",
                ProductId = 1,
                Quantity = 5
            };
        }

        protected override IValidator<ProcessReleaseOperationCommand> CreateValidator(IOperationService operationSrv, IProductService productSrv)
        {
            return new ProcessReleaseOperationCommandValidator(operationSrv, productSrv);
        }

        protected override OperationType GetInvalidOperationType()
        {
            return OperationType.Delivery;
        }

        protected override OperationType GetValidOperationType()
        {
            return OperationType.Release;
        }
    }
}