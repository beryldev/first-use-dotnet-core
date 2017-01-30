using Moq;
using Wrhs.Common;
using Xunit;

namespace Wrhs.Tests
{
    public class RemoveDocumentCmdValidatorTests 
        : CommandValidatorTestsBase<RemoveDocumentCommand>
    {
        private readonly Mock<IDocumentService> documentSrvMock;
        private readonly RemoveDocumentCommand command;

        public RemoveDocumentCmdValidatorTests()
        {
            documentSrvMock = new Mock<IDocumentService>();
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{State = DocumentState.Open});

            command = new RemoveDocumentCommand { DocumentId = 1};
            validator = new RemoveDocumentCommandValidator(documentSrvMock.Object);
        }

        [TheoryAttribute]
        [InlineDataAttribute(0)]
        [InlineDataAttribute(-1)]
        [InlineDataAttribute(-25)]
        public void ShouldReturnErrorWhenInvalidDocumentId(int id)
        {
            command.DocumentId = id;

            var result = Act(command);

            AssertSingleError(result, "DocumentId");
        }

        [Theory]
        [InlineDataAttribute(DocumentState.Executed)]
        [InlineDataAttribute(DocumentState.Confirmed)]
        public void ShoundReturnErrorWhenInvalidDocumentState(DocumentState state)
        {
           documentSrvMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document{State = state});

            var result = Act(command);

            AssertSingleError(result, "DocumentId");

        }
    }
}