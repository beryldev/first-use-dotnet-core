using System;
using Moq;
using NUnit.Framework;
using Wrhs.Operations;
using Wrhs.Operations.Relocation;

namespace Wrhs.Tests
{
    [TestFixture]
    public class RelocationOperationTests
    {
        [Test]
        public void OperationCanBasedOnRelocationDocument()
        {
            var relocationDoc = MakeRelocationDocument();
            var operation = new RelocationOperation();

            operation.SetBaseDocument(relocationDoc);

            Assert.AreEqual(relocationDoc, operation.BaseDocument);
        }

        [Test]
        public void CanAccessDirectlyToBaseRelocationDocument()
        {
            var relocDoc = MakeRelocationDocument();
            var operation = new RelocationOperation();

            operation.BaseRelocationDocument = relocDoc;

            Assert.AreEqual(relocDoc, operation.BaseDocument);
        }

        [Test]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new RelocationOperation();

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(mock.Object); 
            });
        }

        [Test]
        public void RelocateItem()
        {
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            
            operation.RelocateItem(document.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", 1);

            Assert.AreEqual(2, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantRelocateMoreThanOnDocument()
        {
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(document.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", 2);
            });

            Assert.AreEqual(0, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantRelocateNonPresentOnDocProduct()
        {
            var product = new Product();
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(product, "LOC-001-01-1", "LOC-001-01-2", 1);
            });

            Assert.AreEqual(0, operation.PendingAllocations.Count);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-0.01)]
        [TestCase(-1)]
        [TestCase(-9)]
        public void CantRelocateZeroOrLess(decimal quantity)
        {
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(document.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", quantity);
            });

            Assert.AreEqual(0, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantPerformWhenExistsNonRelocatedItems()
        {
            var mock = new Mock<IAllocationService>();
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            Assert.Throws<InvalidOperationException>(()=>
            {
                operation.Perform(mock.Object);
            });
        }

        public void SourceLocationCantBeDestination()
        {
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(document.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-1", 1);
            });
        }

        protected RelocationDocument MakeRelocationDocument()
        {
            var product = new Product()
            {
                Name =  "Product 1",
                Code = "PROD1",
                EAN = "123456789012",
                SKU = "P1234"
            };

            var docLine = new RelocationDocumentLine()
            {
                Product = product,
                Quantity = 1,
                From = "LOC-001-01-1",
                To = "LOC-001-01-2"
            };

            var doc = new RelocationDocument();
            doc.Lines.Add(docLine);

            return doc;
        }
    }
}