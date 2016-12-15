using Wrhs.Common;

namespace Wrhs.Delivery
{
    public class ExecuteDeliveryOperationCommandValidator
        : ExecuteOperationCommandValidator<ExecuteDeliveryOperationCommand>
    {
        public ExecuteDeliveryOperationCommandValidator(IOperationService operationSrv) 
            : base(operationSrv)
        {
        }

        protected override OperationType GetExpectedOperationType()
        {
            return OperationType.Delivery;
        }
    }
}