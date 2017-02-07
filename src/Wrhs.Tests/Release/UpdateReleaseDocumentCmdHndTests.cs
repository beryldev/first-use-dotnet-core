using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;

namespace Wrhs.Tests.Release
{
    public class UpdateReleaseDocumentCmdHndTests
        : UpdateDocumentCmdHndTestsBase<UpdateReleaseDocumentCommand>
    {
        protected override void AssertUpdatedDocument(Document document)
        {
            document.Remarks.Should().Be("some remarks");
            var line = document.Lines.First();
            line.ProductId.Should().Be(1);
            line.Quantity.Should().Be(20);
            line.SrcLocation.Should().Be("src");
        }

        protected override UpdateReleaseDocumentCommand CreateCommand()
        {
            return new UpdateReleaseDocumentCommand
            {
                DocumentId = 1,
                Lines =  new List<CreateDocumentCommand.DocumentLine>
                {
                    new CreateReleaseDocumentCommand.DocumentLine
                    {
                        ProductId = 1,
                        Quantity = 20,
                        SrcLocation = "src"
                    }
                },
                Remarks = "some remarks"
            };
        }

        protected override ICommandHandler<UpdateReleaseDocumentCommand> CreateHandler()
        {
            return new UpdateReleaseDocumentCommandHandler(validatorMock.Object, 
                eventBusMock.Object, docServiceMock.Object);
        }
    }
}