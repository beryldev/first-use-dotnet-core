using Wrhs.Common;

namespace Wrhs.Release
{
    public class ExecuteReleaseOperationCommandValidator
        : ExecuteOperationCommandValidator<ExecuteReleaseOperationCommand>
    {
        public ExecuteReleaseOperationCommandValidator(IOperationService operationSrv) 
            : base(operationSrv)
        {
        }

        protected override OperationType GetExpectedOperationType()
        {
            return OperationType.Release;
        }
    }
}