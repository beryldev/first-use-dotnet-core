using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Xunit;

namespace Wrhs.Tests
{
    public class RemoveDocumentCmdHndTests
    {
        private readonly Mock<IEventBus> eventBusMock;
        private readonly Mock<IValidator<RemoveDocumentCommand>> validatorMock;
        private readonly Mock<IDocumentService> docServiceMock;
        private readonly RemoveDocumentCommand command;
        private readonly RemoveDocumentCommandHandler handler;

        public RemoveDocumentCmdHndTests()
        {
            eventBusMock = new Mock<IEventBus>();
            validatorMock = new Mock<IValidator<RemoveDocumentCommand>>();
            docServiceMock = new Mock<IDocumentService>();
            docServiceMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>())).Returns(new Document(){Id = 1});

            command = new RemoveDocumentCommand { DocumentId = 1};
            handler = new RemoveDocumentCommandHandler(validatorMock.Object, eventBusMock.Object, docServiceMock.Object);
        }

        [Fact]
        public void ShouldRemoveDocumentOnHandleWhenValidCommand()
        {
            validatorMock.Setup(m=>m.Validate(command)).Returns(new List<ValidationResult>());

            handler.Handle(command);

            docServiceMock.Verify(m=>m.Delete(It.IsNotNull<Document>()), Times.Once());
        }

        [Fact]
        public void ShouldPublishEventWithRemovedDocument()
        {
            var validDocId = false;
            validatorMock.Setup(m=>m.Validate(command)).Returns(new List<ValidationResult>());
            eventBusMock.Setup(m=>m.Publish(It.IsNotNull<RemoveDocumentEvent>()))
                .Callback((RemoveDocumentEvent @event)=>{
                    validDocId = @event.RemovedDocument.Id == 1;
                });

            handler.Handle(command);

            validDocId.Should().BeTrue();
        }

        [Fact]
        public void ShouldOnlyThrowValidationExceptionOnHandleWhenInvalidCommand()
        {
            validatorMock.Setup(m=>m.Validate(command)).Returns(new List<ValidationResult>{new ValidationResult("F", "M")});

            Assert.Throws<CommandValidationException>(()=>{
                handler.Handle(command);
            });

            docServiceMock.Verify(m=>m.Delete(It.IsAny<Document>()), Times.Never);
            eventBusMock.Verify(m=>m.Publish(It.IsAny<RemoveDocumentEvent>()), Times.Never);
        }

        
    }
}