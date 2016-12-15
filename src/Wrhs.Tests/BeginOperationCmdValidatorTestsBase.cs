using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class BeginOperationCmdValidatorTestsBase<T>
        where T : BeginOperationCommand
    {
        private readonly Mock<IDocumentService> documentSrvMock;
        private readonly Mock<IOperationService> operationSrvMock;
        private readonly IValidator<T> validator;

        protected BeginOperationCmdValidatorTestsBase()
        {
            documentSrvMock = new Mock<IDocumentService>();
            documentSrvMock.Setup(m=>m.CheckDocumentExistsById(It.IsAny<int>())).Returns(true);
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsAny<int>()))
            .Returns(new Document{State=DocumentState.Confirmed, Type = GetValidDocumentType()});
            operationSrvMock = new Mock<IOperationService>();
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>()))
                .Returns(false);
            validator = CreateValidator(documentSrvMock.Object, operationSrvMock.Object);
        }

        protected abstract IValidator<T> CreateValidator(IDocumentService documentSrv,
            IOperationService operationSrv);

        protected abstract T CreateCommand();

        protected abstract DocumentType GetValidDocumentType();

        protected abstract DocumentType GetInvalidDocumentType();

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShoudReturnValidationErrorWhenInvalidDocumentId(int documentId)
        {
            documentSrvMock.Setup(m=>m.CheckDocumentExistsById(It.IsAny<int>())).Returns(false);
            var command = CreateCommand();
            command.DocumentId = documentId;

            var results = validator.Validate(command);

            results.Should().NotBeNullOrEmpty();
            results.Select(x=>x.Field).Should().Contain("DocumentId");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenDocumentIsOtherThanConfirmed()
        {
            var command = CreateCommand();
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsAny<int>()))
                .Returns(new Document{State=DocumentState.Executed, Type=GetValidDocumentType()});
            
            var results = validator.Validate(command);

            results.Should().HaveCount(1);
            results.Select(x=>x.Field).Should().Contain("DocumentId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnValidationErrorWhenInvalidOperationGuid(string guid)
        {
            var command = CreateCommand();
            command.OperationGuid = guid;

            var results = validator.Validate(command);

            results.Should().HaveCount(1);
            results.Select(x=>x.Field).Should().Contain("OperationGuid");
        }

        [Fact]
        public void ShouldReturnValidationErrorWhenExistsOperationWithPassedGuid()
        {
            var command = CreateCommand();
            operationSrvMock.Setup(m=>m.CheckOperationGuidExists(It.IsAny<string>()))
                .Returns(true);

            var results = validator.Validate(command);

            results.Should().HaveCount(1);
            results.Select(x=>x.Field).Should().Contain("OperationGuid");
        }

        [Fact]
        public void ShouldReturnErrorWhenInvalidDocumentType()
        {
            var command = CreateCommand();
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsAny<int>()))
                .Returns(new Document{ Type = GetInvalidDocumentType(), State=DocumentState.Confirmed});

            var results = validator.Validate(command);

            results.Should().HaveCount(1);
            results.Select(x=>x.Field).Should().Contain("OperationGuid");
        }
    }
}