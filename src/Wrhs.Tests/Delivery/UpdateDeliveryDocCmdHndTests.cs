using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;

namespace Wrhs.Tests.Relocation
{
    public class UpdateDeliveryDocumentCmdHndTest
        : UpdateDocumentCmdHndTestsBase<UpdateDeliveryDocumentCommand>
    {
        protected override UpdateDeliveryDocumentCommand CreateCommand()
        {
            return new UpdateDeliveryDocumentCommand
            {
                DocumentId = 1,
                Lines =  new List<CreateDocumentCommand.DocumentLine>
                {
                    new CreateDocumentCommand.DocumentLine
                    {
                        ProductId = 1,
                        Quantity = 10,
                        DstLocation = "dst"
                    }
                },
                Remarks = "some remarks"
            };
        }

        protected override ICommandHandler<UpdateDeliveryDocumentCommand> CreateHandler()
        {
            return new UpdateDeliveryDocumentCommandHandler(validatorMock.Object, 
                eventBusMock.Object, docServiceMock.Object);
        }

        protected override void AssertUpdatedDocument(Document document)
        {
            document.Remarks.Should().Be("some remarks");
            var line = document.Lines.First();
            line.ProductId.Should().Be(1);
            line.Quantity.Should().Be(10);
            line.DstLocation.Should().Be("dst");
        }
    }
}