using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class CreateDocumentCmdHndTestsBase<TCommand, TEvent> 
        where TCommand : CreateDocumentCommand
        where TEvent : IEvent
    {
        protected readonly TCommand command;
        protected readonly Mock<IValidator<TCommand>> validatorMock;
        protected readonly Mock<IDocumentService> docServiceMock;
        protected readonly Mock<IEventBus> eventBusMock;
        protected readonly ICommandHandler<TCommand> handler;

        protected CreateDocumentCmdHndTestsBase()
        {
            eventBusMock = new Mock<IEventBus>();
            docServiceMock = new Mock<IDocumentService>();
            validatorMock = new Mock<IValidator<TCommand>>();
            validatorMock.Setup(m=>m.Validate(It.IsAny<TCommand>()))
                .Returns(new List<ValidationResult>());

            command = CreateCommand();
            handler = CreateCommandHandler(validatorMock.Object, 
                eventBusMock.Object, docServiceMock.Object);
        }

        protected abstract TCommand CreateCommand();
        protected abstract ICommandHandler<TCommand> CreateCommandHandler(IValidator<TCommand> validator,
            IEventBus eventBus, IDocumentService docService);
        protected abstract DocumentType GetDocumentType();
        
        [Fact]
        public void ShouldSaveNewDocWhenCmdValid()
        { 
            var isValidDocumentType = false;
            var passedRemarks = false;
            var isValidState = false;
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<TCommand>()))
                .Returns(new List<ValidationResult>());
            docServiceMock.Setup(m=>m.Save(It.IsNotNull<Document>()))
                .Callback((Document doc) => {
                    isValidDocumentType = doc.Type == GetDocumentType();
                    passedRemarks = !string.IsNullOrEmpty(doc.Remarks);
                    isValidState = doc.State == DocumentState.Open;
                 });

            command.Remarks = "some remarks";
            handler.Handle(command);

            docServiceMock.Verify(m=>m.Save(It.IsNotNull<Document>()), Times.Once());
            isValidDocumentType.Should().BeTrue();
            passedRemarks.Should().BeTrue();
        }

        [Fact]
        public void ShouldPublishEventAfterSave()
        {
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<TCommand>()))
                .Returns(new List<ValidationResult>());

            handler.Handle(command);

            eventBusMock.Verify(m=>m.Publish(It.IsAny<TEvent>()), Times.Once());
        }

        [Fact]
        public void ShouldOnlyThrowCommandValidationExceptionWhenValidationFails()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<TCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("SomeField", "Some message.")});

            Assert.Throws<CommandValidationException>(()=>
            {
                handler.Handle(command);
            });

            docServiceMock.Verify(m=>m.Save(It.IsNotNull<Document>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsNotNull<TEvent>()), Times.Never());
        }
    }
}