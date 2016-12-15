using Wrhs.Common;

namespace Wrhs.Relocation
{
    public class ExecuteRelocationOperationCommandValidator
        : ExecuteOperationCommandValidator<ExecuteRelocationOperationCommand>
    {
        public ExecuteRelocationOperationCommandValidator(IOperationService operationSrv) 
            : base(operationSrv)
        {
        }

        protected override OperationType GetExpectedOperationType()
        {
            return OperationType.Relocation;
        }
    }
}