using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;

namespace Wrhs.Tests.Delivery
{
    public class BeginDeliveryOperationCmdHndTests
        : BeginOperationCmdHndTestsBase<BeginDeliveryOperationCommand, BeginDeliveryOperationEvent>
    {
        protected override BeginDeliveryOperationCommand CreateCommand()
        {
            return new BeginDeliveryOperationCommand{ DocumentId = 1, OperationGuid = "some-guid"};
        }

        protected override BeginOperationCommandHandler<BeginDeliveryOperationCommand, BeginDeliveryOperationEvent> CreateCommandHandler(IValidator<BeginDeliveryOperationCommand> validator, IEventBus eventBus, IOperationPersist operationPersist)
        {
            return new BeginDeliveryOperationCommandHandler(validatorMock.Object, eventBusMock.Object,
                operationPersistMock.Object);
        }

        protected override OperationType GetValidOperationType()
        {
            return OperationType.Delivery;
        }
    }
}