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
        private readonly Mock<IDocumentService> docServiceMock;
        private readonly RemoveDocumentCommand command;
        private readonly RemoveDocumentCommandHandler handler;

        public RemoveDocumentCmdHndTests()
        {
            eventBusMock = new Mock<IEventBus>();
            docServiceMock = new Mock<IDocumentService>();
            docServiceMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>())).Returns(new Document(){Id = 1});

            command = new RemoveDocumentCommand { DocumentId = 1};
            handler = new RemoveDocumentCommandHandler(eventBusMock.Object, docServiceMock.Object);
        }

        [Fact]
        public void ShouldRemoveDocumentOnHandle()
        {
            handler.Handle(command);

            docServiceMock.Verify(m=>m.Delete(It.IsNotNull<Document>()), Times.Once());
        }

        [Fact]
        public void ShouldPublishEventWithRemovedDocument()
        {
            var validDocId = false;
            eventBusMock.Setup(m => m.Publish(It.IsNotNull<RemoveDocumentEvent>()))
                .Callback((RemoveDocumentEvent @event) =>
                {
                    validDocId = @event.RemovedDocument.Id == 1;
                });

            handler.Handle(command);

            validDocId.Should().BeTrue();
        }
    }
}