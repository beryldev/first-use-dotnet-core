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
        protected readonly Mock<IDocumentPersist> docPersistMock;
        protected readonly Mock<IEventBus> eventBusMock;
        protected readonly ICommandHandler<TCommand> handler;

        protected CreateDocumentCmdHndTestsBase()
        {
            eventBusMock = new Mock<IEventBus>();
            docPersistMock = new Mock<IDocumentPersist>();
            validatorMock = new Mock<IValidator<TCommand>>();
            validatorMock.Setup(m=>m.Validate(It.IsAny<TCommand>()))
                .Returns(new List<ValidationResult>());

            command = CreateCommand();
            handler = CreateCommandHandler(validatorMock.Object, 
                eventBusMock.Object, docPersistMock.Object);
        }

        protected abstract TCommand CreateCommand();
        protected abstract ICommandHandler<TCommand> CreateCommandHandler(IValidator<TCommand> validator,
            IEventBus eventBus, IDocumentPersist docPersist);
        protected abstract DocumentType GetDocumentType();
        
        [Fact]
        public void ShouldSaveNewDocWhenCmdValid()
        { 
            var isValidDocumentType = false;
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<TCommand>()))
                .Returns(new List<ValidationResult>());
            docPersistMock.Setup(m=>m.Save(It.IsNotNull<Document>()))
                .Callback((Document doc) => {
                    isValidDocumentType = doc.Type == GetDocumentType();
                 });

            handler.Handle(command);

            docPersistMock.Verify(m=>m.Save(It.IsNotNull<Document>()), Times.Once());
            isValidDocumentType.Should().BeTrue();
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

            docPersistMock.Verify(m=>m.Save(It.IsNotNull<Document>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsNotNull<TEvent>()), Times.Never());
        }
    }
}