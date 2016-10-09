using System;
using System.Collections.Generic;
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
        public void CantReleaseAtOneTimeMoreThanOnBaseDocument()
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.ReleaseItem(document.Lines[0].Product, "LOC-001-01", 2);
            });

            Assert.AreEqual(0, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantReleaseCombinedMoreThanOnBaseDocument()
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation(document);

            operation.ReleaseItem(document.Lines[0].Product, "LOC-001-01", 1);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.ReleaseItem(document.Lines[0].Product, "LOC-001-01", 1);
            });

            Assert.AreEqual(1, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantReleaseResourceNonPresentOnDocument()
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation(document);
            var product = new Product { Code = "PROD2", Name="Product 2", EAN="111222333222" };

            Assert.Throws<ArgumentException>(()=>
            {
                operation.ReleaseItem(product, "LOC-001-01", 1);
            });

            Assert.AreEqual(0, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantReleaseFromLocationNonPresentAtDocument()
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.ReleaseItem(document.Lines[0].Product, "LOC-001-004", 1);
            });

             Assert.AreEqual(0, operation.PendingAllocations.Count);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-0.001)]
        [TestCase(-100)]
        public void CantReleaseZeroOrLess(decimal quantity)
        {
            var document = MakeReleaseDocument();
            var operation = MakeReleaseOperation(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.ReleaseItem(document.Lines[0].Product, "LOC-001-01", quantity);
            });

            Assert.AreEqual(0, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantPerformWhenExistsNonRelocatedItems()
        {
            var items = new List<Allocation>();
            var mock = new Mock<IAllocationService>();
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

        protected Mock<IAllocationService> MakeAllocationServiceMock(List<Allocation> items)
        {
            
        }
    }
}