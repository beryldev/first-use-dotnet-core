using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Xunit;

namespace Wrhs.Tests
{
    public class ChangeDocStateCmdHndTests
    {
        private readonly Mock<IEventBus> fakeEventBus;
        private readonly Mock<IDocumentService> fakeDocSrv;
        private readonly ChangeDocStateCommand command;
        private readonly ChangeDocStateCommandHandler handler;

        public ChangeDocStateCmdHndTests()
        {
            fakeEventBus = new Mock<IEventBus>();
            fakeDocSrv = new Mock<IDocumentService>();
            fakeDocSrv.Setup(m=>m.GetDocumentById(It.IsNotNull<int>())).Returns(new Document(){Id = 1});

            command = new ChangeDocStateCommand { DocumentId = 1 };
            handler = new ChangeDocStateCommandHandler(fakeEventBus.Object, fakeDocSrv.Object);
        }

        [Fact]
        public void ShouldUpdateDocumentStateOnHandle() 
        {
            var isValidId = false;
            var newState = DocumentState.Open;
            command.NewState = DocumentState.Confirmed;
            fakeDocSrv.Setup(m=>m.Update(It.IsNotNull<Document>()))
                .Callback((Document doc) => {
                    isValidId = doc.Id == 1;
                    newState = doc.State;
                });

            handler.Handle(command);

            isValidId.Should().BeTrue();
            newState.Should().Be(DocumentState.Confirmed);
        }

        [Fact]
        public void ShouldPublishEventAfterHandleCommand() 
        { 
            var @event = new ChangeDocStateEvent();
            command.NewState = DocumentState.Confirmed;
            fakeEventBus.Setup(m=>m.Publish(It.IsNotNull<ChangeDocStateEvent>()))
                .Callback((ChangeDocStateEvent evt) => @event = evt);

            handler.Handle(command);

            @event.DocumentId.Should().Be(1);
            @event.NewState.Should().Be(DocumentState.Confirmed);
            @event.OldState.Should().Be(DocumentState.Open);
        }
    }
}