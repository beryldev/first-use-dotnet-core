using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;

namespace Wrhs.Tests.Relocation
{
    public class CreateRelocationDocumentCmdHndTests
        : CreateDocumentCmdHndTestsBase<CreateRelocationDocumentCommand, CreateRelocationDocumentEvent>
    {
        protected override CreateRelocationDocumentCommand CreateCommand()
        {
            return new CreateRelocationDocumentCommand()
            {
                Lines = new List<CreateDocumentCommand.DocumentLine>()
            };
        }

        protected override ICommandHandler<CreateRelocationDocumentCommand> CreateCommandHandler(IValidator<CreateRelocationDocumentCommand> validator, IEventBus eventBus, IDocumentPersist docPersist)
        {
            return new CreateRelocationDocumentCommandHandler(validator, eventBus, docPersist);
        }

        protected override DocumentType GetDocumentType()
        {
            return DocumentType.Relocation;
        }
    }
}