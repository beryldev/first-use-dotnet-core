using System.Linq;
using Wrhs.Core;
using Wrhs.Documents;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class DocumentRegistratorTests<T> where T : IEntity, INumerableDocument
    {
        [Fact]
        public void ShouldInsertDocumentToRepositoryWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document);

            Assert.Equal(1, repository.Get().Count());
        }

        [Fact]
        public void ShouldAssignNumberToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document);

            Assert.NotEqual(0, document.Number);
        }

        [Fact]
        public void ShouldAssignFullNumberToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document);

            Assert.NotNull(document.FullNumber);
            Assert.NotEmpty(document.FullNumber);
        }

        [Fact]
        public void ShouldAssignFullNumberWithCorrectPefixToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document);

            Assert.StartsWith(GetDocumentPrefix(), document.FullNumber);
        }

        [Fact]
        public void ShouldAssignUniqueFullNumberToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document1 = CreateDocument();
            var document2 = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document1);
            registrator.Register(document2);

            Assert.Equal(2, repository.Get().Count());
            Assert.NotEqual(document1.FullNumber, document2.FullNumber);
        }

        protected abstract string GetDocumentPrefix();

        protected abstract T CreateDocument();
    }
}