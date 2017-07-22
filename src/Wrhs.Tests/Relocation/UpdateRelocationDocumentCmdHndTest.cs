using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;

namespace Wrhs.Tests.Relocation
{
    public class UpdateRelocationDocumentCmdHndTest
        : UpdateDocumentCmdHndTestsBase<UpdateRelocationDocumentCommand>
    {
        protected override UpdateRelocationDocumentCommand CreateCommand()
        {
            return new UpdateRelocationDocumentCommand
            {
                DocumentId = 1,
                Lines =  new List<CreateDocumentCommand.DocumentLine>
                {
                    new CreateRelocationDocumentCommand.DocumentLine
                    {
                        ProductId = 1,
                        Quantity = 10,
                        SrcLocation = "src",
                        DstLocation = "dst"
                    }
                },
                Remarks = "some remarks"
            };
        }

        protected override ICommandHandler<UpdateRelocationDocumentCommand> CreateHandler()
        {
            return new UpdateRelocationDocumentCommandHandler(eventBusMock.Object, docServiceMock.Object);
        }

        protected override void AssertUpdatedDocument(Document document)
        {
            document.Remarks.Should().Be("some remarks");
            var line = document.Lines.First();
            line.ProductId.Should().Be(1);
            line.Quantity.Should().Be(10);
            line.SrcLocation.Should().Be("src");
            line.DstLocation.Should().Be("dst");
        }
    }
}