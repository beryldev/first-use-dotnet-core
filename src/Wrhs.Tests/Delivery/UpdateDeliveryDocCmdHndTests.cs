using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Delivery;
using Xunit;

namespace Wrhs.Tests.Delivery
{
    public class UpdateDeliveryDocCmdHndTests
    {
        private readonly Mock<IEventBus> eventBusMock;
        private readonly Mock<IDocumentService> docServiceMock;
        private readonly Mock<IDocumentPersist> docPersistMock;
        private readonly Mock<IValidator<UpdateDeliveryDocumentCommand>> validatorMock;
    
        private readonly UpdateDeliveryDocumentCommand command;
        private readonly UpdateDeliveryDocumentCommandHandler handler;

        public UpdateDeliveryDocCmdHndTests()
        {
            eventBusMock = new Mock<IEventBus>();
            docServiceMock = new Mock<IDocumentService>();
            docPersistMock = new Mock<IDocumentPersist>();
            validatorMock = new Mock<IValidator<UpdateDeliveryDocumentCommand>>();
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<UpdateDeliveryDocumentCommand>()))
                .Returns(new List<ValidationResult>());


            command = new UpdateDeliveryDocumentCommand
            {
                DocumentId = 1,
                Lines = new List<CreateDocumentCommand.DocumentLine>
                {
                    new CreateDeliveryDocumentCommand.DocumentLine()
                },
                Remarks = "some remarks"
            };
            handler = new UpdateDeliveryDocumentCommandHandler(validatorMock.Object, 
                eventBusMock.Object, docPersistMock.Object, docServiceMock.Object);
        }

        [Fact]
        public void ShouldUpdateDocumentOnWHandleWhenValidCommand()
        {
            var docId = 0;
            var lines = new List<DocumentLine>();
            var remarks = string.Empty;
            docServiceMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{ Id = 1, Lines = new List<DocumentLine>()});
            docPersistMock.Setup(m=>m.Update(It.IsNotNull<Document>()))
                .Callback((Document doc)=>{
                    docId = doc.Id;
                    lines = doc.Lines;
                    remarks = doc.Remarks;
                });

            handler.Handle(command);

            docId.Should().Be(1);
            lines.Should().HaveCount(1);
            remarks.Should().Be("some remarks");
        }

        [Fact]
        public void ShouldPublishEventAfterHandleValidCommand()
        {
            var docId = 0;
            docServiceMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{ Id = 1, Lines = new List<DocumentLine>()});

            eventBusMock.Setup(m=>m.Publish(It.IsAny<UpdateDeliveryDocumentEvent>()))
                .Callback((UpdateDeliveryDocumentEvent @event) => {
                    docId = @event.DocumentId;
                });

            handler.Handle(command);
            
            docId.Should().Be(1);
        }

        [Fact]
        public void ShouldOnlyThrowExceptionOnHandleInvalidCommand()
        {
            docServiceMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{ Id = 1, Lines = new List<DocumentLine>()});
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<UpdateDeliveryDocumentCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("F", "M")});

            Assert.Throws<CommandValidationException>(()=>{
                handler.Handle(command);
            });

            docPersistMock.Verify(m=>m.Update(It.IsNotNull<Document>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsNotNull<UpdateDeliveryDocumentEvent>()), Times.Never());
        }
    }
}