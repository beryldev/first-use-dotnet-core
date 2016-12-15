using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;

namespace Wrhs.Tests.Delivery
{
    public class ExecuteDeliveryOperationCmdHndTests
        : ExecuteOperationCmdHndTestsBase<ExecuteDeliveryOperationCommand, ExecuteDeliveryOperationEvent>
    {
        protected override ExecuteDeliveryOperationCommand CreateCommand()
        {
            return new ExecuteDeliveryOperationCommand { OperationGuid = "some-guid"};
        }

        protected override ICommandHandler<ExecuteDeliveryOperationCommand> CreateHandler(IValidator<ExecuteDeliveryOperationCommand> validator, IEventBus eventBus, HandlerParameters parameters)
        {
            return new ExecuteDeliveryOperationCommandHandler(validator, eventBus, parameters);
        }

        protected override DocumentType GetExpectedDocumentType()
        {
            return DocumentType.Delivery;
        }

        protected override OperationType GetExpectedOperationType()
        {
            return OperationType.Delivery;
        }
    }
}