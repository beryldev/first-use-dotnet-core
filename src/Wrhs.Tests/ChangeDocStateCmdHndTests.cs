using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Xunit;

namespace Wrhs.Tests
{
    public class ChangeDocStateCmdHndTests
    {
        private readonly Mock<IEventBus> eventBusMock;
        private readonly Mock<IValidator<ChangeDocStateCommand>> validatorMock;
        private readonly Mock<IDocumentService> docServiceMock;
        private readonly Mock<IDocumentPersist> docPersistMock;
        private readonly ChangeDocStateCommand command;
        private readonly ChangeDocStateCommandHandler handler;

        public ChangeDocStateCmdHndTests()
        {
            eventBusMock = new Mock<IEventBus>();
            validatorMock = new Mock<IValidator<ChangeDocStateCommand>>();
            docPersistMock = new Mock<IDocumentPersist>();
            docServiceMock = new Mock<IDocumentService>();
            docServiceMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>())).Returns(new Document(){Id = 1});

            command = new ChangeDocStateCommand { DocumentId = 1 };
            handler = new ChangeDocStateCommandHandler(validatorMock.Object, eventBusMock.Object,
                docPersistMock.Object, docServiceMock.Object);
        }

        [Fact]
        public void ShouldUpdateDocumentStateWhenValidCommand() 
        {
            var isValidId = false;
            var newState = DocumentState.Open;
            command.NewState = DocumentState.Confirmed;
            validatorMock.Setup(m=>m.Validate(command)).Returns(new List<ValidationResult>());
            docPersistMock.Setup(m=>m.Update(It.IsNotNull<Document>()))
                .Callback((Document doc) => {
                    isValidId = doc.Id == 1;
                    newState = doc.State;
                });

            handler.Handle(command);

            isValidId.Should().BeTrue();
            newState.Should().Be(DocumentState.Confirmed);
        }

        [Fact]
        public void ShouldPublishEventAfterHandleValidCommand() 
        { 
            var @event = new ChangeDocStateEvent();
            command.NewState = DocumentState.Confirmed;
            validatorMock.Setup(m=>m.Validate(command)).Returns(new List<ValidationResult>());
            eventBusMock.Setup(m=>m.Publish(It.IsNotNull<ChangeDocStateEvent>()))
                .Callback((ChangeDocStateEvent evt) => @event = evt);

            handler.Handle(command);

            @event.DocumentId.Should().Be(1);
            @event.NewState.Should().Be(DocumentState.Confirmed);
            @event.OldState.Should().Be(DocumentState.Open);
        }

        [Fact]
        public void ShouldOnlyThrowExceptionWhenInvalidCommnad() 
        { 
            validatorMock.Setup(m=>m.Validate(command)).Returns(new List<ValidationResult>{new ValidationResult("F", "M")});

            Assert.Throws<CommandValidationException>(()=>
            {
                handler.Handle(command);
            });

            docPersistMock.Verify(m=>m.Update(It.IsNotNull<Document>()), Times.Never);
            eventBusMock.Verify(m=>m.Publish(It.IsNotNull<ChangeDocStateEvent>()), Times.Never);
        }
    }
}