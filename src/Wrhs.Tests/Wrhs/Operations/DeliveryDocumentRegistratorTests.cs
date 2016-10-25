using System.Linq;
using NUnit.Framework;
using Wrhs.Operations.Delivery;

namespace Wrhs.Tests
{
    [TestFixture]
    public class DeliveryDocumentRegistratorTests
    {
        [Test]
        public void ShouldInsertDocumentToRepositoryWhenRegister()
        {
            var repository = RepositoryFactory<DeliveryDocument>.Make();
            var document = new DeliveryDocument();
            var registrator = new DeliveryDocumentRegistrator(repository);

            registrator.Register(document);

            Assert.AreEqual(1, repository.Get().Count());
        }

        [Test]
        public void ShouldAssignFullNumberToDocumentWhenRegister()
        {
            var repository = RepositoryFactory<DeliveryDocument>.Make();
            var document = new DeliveryDocument();
            var registrator = new DeliveryDocumentRegistrator(repository);

            registrator.Register(document);

            Assert.IsNotNull(document.FullNumber);
            Assert.IsNotEmpty(document.FullNumber);
        }
    }
}