using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;

namespace Wrhs.Tests.Release
{
    public class ExecuteReleaseOperationCmdValidatorTests
        : ExecuteOperationCmdValidatorTestsBase<ExecuteReleaseOperationCommand>
    {
        protected override ExecuteReleaseOperationCommand CreateCommand()
        {
            return new ExecuteReleaseOperationCommand { OperationGuid = "some-guid" };
        }

        protected override IValidator<ExecuteReleaseOperationCommand> CreateValidator(IOperationService operationSrv)
        {
            return new ExecuteReleaseOperationCommandValidator(operationSrv);
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