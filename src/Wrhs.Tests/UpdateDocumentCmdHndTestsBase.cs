using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class UpdateDocumentCmdHndTestsBase<TCommand>
        where TCommand : ICommand
    {
        protected readonly Mock<IEventBus> eventBusMock;
        protected readonly Mock<IDocumentService> docServiceMock;
        protected readonly Mock<IValidator<TCommand>> validatorMock;
    
        protected readonly TCommand command;
        protected readonly ICommandHandler<TCommand> handler;

        protected UpdateDocumentCmdHndTestsBase()
        {
            eventBusMock = new Mock<IEventBus>();
            docServiceMock = new Mock<IDocumentService>();
            validatorMock = new Mock<IValidator<TCommand>>();
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<TCommand>()))
                .Returns(new List<ValidationResult>());

            command = CreateCommand();
            handler = CreateHandler();
        }

        protected abstract TCommand CreateCommand();

        protected abstract ICommandHandler<TCommand> CreateHandler();

        protected abstract void AssertUpdatedDocument(Document document);

        [Fact]
        public void ShouldUpdateDocumentOnWHandleWhenValidCommand()
        {
            Document document = null;

            docServiceMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{ Id = 1, Lines = new List<DocumentLine>()});
            docServiceMock.Setup(m=>m.Update(It.IsNotNull<Document>()))
                .Callback((Document doc)=>{
                    document = doc;
                });

            handler.Handle(command);

            AssertUpdatedDocument(document);
        }

        [Fact]
        public void ShouldPublishEventAfterHandleValidCommand()
        {
            var docId = 0;
            docServiceMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{ Id = 1, Lines = new List<DocumentLine>()});

            eventBusMock.Setup(m=>m.Publish(It.IsAny<UpdateDocumentEvent>()))
                .Callback((UpdateDocumentEvent @event) => {
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
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<TCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("F", "M")});

            Assert.Throws<CommandValidationException>(()=>{
                handler.Handle(command);
            });

            docServiceMock.Verify(m=>m.Update(It.IsNotNull<Document>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsNotNull<UpdateDocumentEvent>()), Times.Never());
        }
    }
}