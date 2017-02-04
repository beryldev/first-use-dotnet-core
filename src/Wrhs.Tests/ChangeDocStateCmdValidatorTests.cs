using FluentAssertions;
using Moq;
using Wrhs.Common;
using Xunit;

namespace Wrhs.Tests
{
    public class ChangeDocStateCmdValidatorTests
        : CommandValidatorTestsBase<ChangeDocStateCommand>
    {
        private readonly Mock<IDocumentService> documentSrvMock;
        private readonly ChangeDocStateCommand command;
        
        public ChangeDocStateCmdValidatorTests()
        {
            documentSrvMock = new Mock<IDocumentService>();
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{ Id = 1, State = DocumentState.Open});

            validator = new ChangeDocStateCommandValidator(documentSrvMock.Object);
            command = new ChangeDocStateCommand
            {
                DocumentId = 1,
                NewState = DocumentState.Confirmed
            };
        }

        [Theory]
        [InlineData(DocumentState.Confirmed, DocumentState.Open)]
        [InlineData(DocumentState.Executed, DocumentState.Confirmed)]
        [InlineData(DocumentState.Executed, DocumentState.Open)]
        [InlineData(DocumentState.Open, DocumentState.Executed)]
        [InlineData(DocumentState.Confirmed, DocumentState.Executed)]
        [InlineData(DocumentState.Open, DocumentState.Canceled)]
        [InlineData(DocumentState.Executed, DocumentState.Canceled)]
        [InlineData(DocumentState.Canceled, DocumentState.Executed)]
        [InlineData(DocumentState.Canceled, DocumentState.Confirmed)]
        [InlineData(DocumentState.Canceled, DocumentState.Open)]
        public void ShouldReturnErrorWhenCantChangeFromOneStateToOther(DocumentState from, DocumentState to)
        {
            command.NewState = to;
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{ Id = 1, State = from});
          
            var results = Act(command);

            AssertSingleError(results, "NewState");
        }

        [Theory]
        [InlineData(DocumentState.Open, DocumentState.Confirmed)]   
        [InlineData(DocumentState.Open, DocumentState.Open)]     
        public void ShouldNoReturnAnyErrorWhenValidCommand(DocumentState from, DocumentState to)
        {
            command.NewState = to;
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{ Id = 1, State = from});

            var results = Act(command);

            results.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-321)]
        public void ShouldReturnErrorWhenInvalidDocumentId(int documentId)
        {
            command.DocumentId = documentId;

            var results = Act(command);

            AssertSingleError(results, "DocumentId");
        }

        public void ShouldReturnErrorWhenDocumentNotFound()
        {
             documentSrvMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns((Document)null);

            var results = Act(command);

            AssertSingleError(results, "DocumentId");
        }
    }
}