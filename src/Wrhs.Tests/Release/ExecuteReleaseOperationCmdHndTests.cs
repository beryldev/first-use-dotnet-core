using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;

namespace Wrhs.Tests.Release
{
    public class ExecuteReleaseOperationCmdHndTests
        : ExecuteOperationCmdHndTestsBase<ExecuteReleaseOperationCommand, ExecuteReleaseOperationEvent>
    {
        protected override ExecuteReleaseOperationCommand CreateCommand()
        {
            return new ExecuteReleaseOperationCommand { OperationGuid = "some-guid" };
        }

        protected override ICommandHandler<ExecuteReleaseOperationCommand> CreateHandler(IValidator<ExecuteReleaseOperationCommand> validator,
            IEventBus eventBus, HandlerParameters parameters)
        {
            return new ExecuteReleaseOperationCommandHandler(validator,
                eventBus, parameters);
        }

        protected override DocumentType GetExpectedDocumentType()
        {
            return DocumentType.Release;
        }

        protected override OperationType GetExpectedOperationType()
        {
            return OperationType.Release;
        }
    }
}