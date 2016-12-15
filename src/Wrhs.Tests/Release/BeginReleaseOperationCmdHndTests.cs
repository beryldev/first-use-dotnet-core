using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;

namespace Wrhs.Tests.Release
{
    public class BeginReleaseOperationCmdHndTests
        : BeginOperationCmdHndTestsBase<BeginReleaseOperationCommand, BeginReleaseOperationEvent>
    {
        protected override BeginReleaseOperationCommand CreateCommand()
        {
            return new BeginReleaseOperationCommand
            { 
                DocumentId = 1, 
                OperationGuid = "some-guid"
            };
        }

        protected override BeginOperationCommandHandler<BeginReleaseOperationCommand, BeginReleaseOperationEvent> CreateCommandHandler(IValidator<BeginReleaseOperationCommand> validator, IEventBus eventBus, IOperationPersist operationPersist)
        {
            return new BeginReleaseOperationCommandHandler(validator,
                eventBus, operationPersist);
        }

        protected override OperationType GetValidOperationType()
        {
            return OperationType.Release;
        }
    }
}