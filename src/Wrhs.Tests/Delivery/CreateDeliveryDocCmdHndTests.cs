using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;
using Xunit;

namespace Wrhs.Tests.Delivery
{
    public class CreateDeliveryDocCmdHndTests
        : CreateDocumentCmdHndTestsBase<CreateDeliveryDocumentCommand, CreateDeliveryDocumentEvent>
    {

        [Fact]
        public void ShouldRegisterDocumentWithDstLocation()
        {
            var validDstLocation = false;
            var command = CreateCommand();
            (command.Lines as List<CreateDocumentCommand.DocumentLine>)
                .Add(new CreateDocumentCommand.DocumentLine
            {
                DstLocation = "loc001"
            });
            docServiceMock.Setup(m=>m.Save(It.IsNotNull<Document>()))
                .Callback((Document doc)=>{
                    validDstLocation = doc.Lines.First().DstLocation == "loc001";
                });

            handler.Handle(command);

            validDstLocation.Should().BeTrue();
        }

        protected override CreateDeliveryDocumentCommand CreateCommand()
        {
            return new CreateDeliveryDocumentCommand
            {
                Lines = new List<CreateDocumentCommand.DocumentLine>()
            };
        }

        protected override ICommandHandler<CreateDeliveryDocumentCommand> CreateCommandHandler(IValidator<CreateDeliveryDocumentCommand> validator, IEventBus eventBus, IDocumentService docService)
        {
            return new CreateDeliveryDocumentCommandHandler(eventBus, docService);
        }

        protected override DocumentType GetDocumentType()
        {
            return DocumentType.Delivery;
        }
    }
}