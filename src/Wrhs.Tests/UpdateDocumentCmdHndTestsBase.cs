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
    
        protected readonly TCommand command;
        protected readonly ICommandHandler<TCommand> handler;

        protected UpdateDocumentCmdHndTestsBase()
        {
            eventBusMock = new Mock<IEventBus>();
            docServiceMock = new Mock<IDocumentService>();
            
            command = CreateCommand();
            handler = CreateHandler();
        }

        protected abstract TCommand CreateCommand();

        protected abstract ICommandHandler<TCommand> CreateHandler();

        protected abstract void AssertUpdatedDocument(Document document);

        [Fact]
        public void ShouldUpdateDocumentOnWHandle()
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
        public void ShouldPublishEventAfterHandle()
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
    }
}