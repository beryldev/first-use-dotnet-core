using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;

namespace Wrhs.Tests.Relocation
{
    public class ExecuteRelocationOperationCmdValidatorTests
        : ExecuteOperationCmdValidatorTestsBase<ExecuteRelocationOperationCommand>
    {
        protected override ExecuteRelocationOperationCommand CreateCommand()
        {
            return new ExecuteRelocationOperationCommand { OperationGuid = "some-guid" };
        }

        protected override IValidator<ExecuteRelocationOperationCommand> CreateValidator(IOperationService operationSrv)
        {
            return new ExecuteRelocationOperationCommandValidator(operationSrv);
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