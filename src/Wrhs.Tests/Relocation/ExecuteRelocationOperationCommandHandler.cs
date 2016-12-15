using System;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;

namespace Wrhs.Tests.Relocation
{
    public class ExecuteRelocationOperationCmdHndTests
        : ExecuteOperationCmdHndTestsBase<ExecuteRelocationOperationCommand, ExecuteRelocationOperationEvent>
    {
        protected override ExecuteRelocationOperationCommand CreateCommand()
        {
            return new ExecuteRelocationOperationCommand { OperationGuid = "some-guid"};
        }

        protected override ICommandHandler<ExecuteRelocationOperationCommand> CreateHandler(IValidator<ExecuteRelocationOperationCommand> validator, 
            IEventBus eventBus, HandlerParameters parameters)
        {
            return new ExecuteRelocationOperationCommandHandler(validator, eventBus, parameters);
        }

        protected override DocumentType GetExpectedDocumentType()
        {
            return DocumentType.Relocation;
        }

        protected override OperationType GetExpectedOperationType()
        {
            return OperationType.Relocation;
        }
    }
}