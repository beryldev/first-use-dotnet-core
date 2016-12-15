using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;

namespace Wrhs.Tests.Relocation
{
    public class BeginRelocationCmdHndTests
        : BeginOperationCmdHndTestsBase<BeginRelocationOperationCommand, BeginRelocationOperationEvent>
    {
        protected override BeginRelocationOperationCommand CreateCommand()
        {
            return new BeginRelocationOperationCommand{ DocumentId = 1, OperationGuid = "some-guid"};
        }

        protected override BeginOperationCommandHandler<BeginRelocationOperationCommand, BeginRelocationOperationEvent> CreateCommandHandler(IValidator<BeginRelocationOperationCommand> validator, IEventBus eventBus, IOperationPersist operationPersist)
        {
            return new BeginRelocationOperationCommandHandler(validatorMock.Object, eventBusMock.Object,
                operationPersistMock.Object);
        }

        protected override OperationType GetValidOperationType()
        {
            return OperationType.Relocation;
        }
    }
}