using System.Linq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Documents;

namespace Wrhs.Tests
{
    public abstract class DocumentRegistratorTests<T> where T : IEntity, INumerableDocument
    {
        [Test]
        public void ShouldInsertDocumentToRepositoryWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document);

            Assert.AreEqual(1, repository.Get().Count());
        }

        [Test]
        public void ShouldAssignNumberToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document);

            Assert.AreNotEqual(0, document.Number);
        }

        [Test]
        public void ShouldAssignFullNumberToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document);

            Assert.IsNotNull(document.FullNumber);
            Assert.IsNotEmpty(document.FullNumber);
        }

        [Test]
        public void ShouldAssignFullNumberWithCorrectPefixToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document);

            StringAssert.StartsWith(GetDocumentPrefix(), document.FullNumber);
        }

        [Test]
        public void ShouldAssignUniqueFullNumberToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<T>.Make();
            var document1 = CreateDocument();
            var document2 = CreateDocument();
            var registrator = new DocumentRegistrator<T>(repository, GetDocumentPrefix());

            registrator.Register(document1);
            registrator.Register(document2);

            Assert.AreEqual(2, repository.Get().Count());
            Assert.AreNotEqual(document1.FullNumber, document2.FullNumber);
        }

        protected abstract string GetDocumentPrefix();

        protected abstract T CreateDocument();
    }
}