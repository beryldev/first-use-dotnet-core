using System;
using Moq;
using NUnit.Framework;
using Wrhs.Documents;
using Wrhs.Operations;
using Wrhs.Operations.Release;

namespace Wrhs.Tests
{
    [TestFixture]
    public class ReleaseOperationTests
    {
        [Test]
        public void OperationCanBasedOnReleaseDocument()
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation();

            operation.SetBaseDocument(document);

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void CanAccessDirectlyToBaseReleaseDocument()
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation();

            operation.BaseReleaseDocument = document;

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = MakeReleaseOperation();

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(mock.Object); 
            });
        }

        [Test]
        public void CanReleaseItem()
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation(document);

            operation.ReleaseItem(document.Lines[0].Product, "LOC-001-01", 1);

            Assert.AreEqual(1, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantReleaseMoreThanOnBaseDocument()
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation(document);

            
        }

        protected ReleaseDocument MakeReleaseDocument()
        {
            var document = new ReleaseDocument();
            var prod = new Product
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "123456789012"
            };

            document.Lines.Add(new ReleaseDocumentLine
            {
                Product = prod,
                Quantity = 1,
                Location = "LOC-001-01"
            });

            return document;
        }

        protected ReleaseOperation MakeReleaseOperation(ReleaseDocument document)
        {
            var operation = MakeReleaseOperation();
            operation.SetBaseDocument(document);

            return operation;
        }

        protected ReleaseOperation MakeReleaseOperation()
        {
            var operation = new ReleaseOperation();

            return operation;
        }
    }
}