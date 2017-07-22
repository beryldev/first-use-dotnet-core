using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;

namespace Wrhs.Tests.Release
{
    public class CreateReleaseDocumentCmdHndTests
        : CreateDocumentCmdHndTestsBase<CreateReleaseDocumentCommand, CreateReleaseDocumentEvent>
    {
        protected override CreateReleaseDocumentCommand CreateCommand()
        {
            return new CreateReleaseDocumentCommand
            {
                Lines = new List<CreateDocumentCommand.DocumentLine>()
            };
        }

        protected override ICommandHandler<CreateReleaseDocumentCommand> CreateCommandHandler(IValidator<CreateReleaseDocumentCommand> validator, 
            IEventBus eventBus, IDocumentService docService)
        {
            return new CreateReleaseDocumentCommandHandler(eventBus, docService);
        }

        protected override DocumentType GetDocumentType()
        {
            return DocumentType.Release;
        }
    }
}