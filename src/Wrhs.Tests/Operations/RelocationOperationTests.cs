using System;
using System.Collections.Generic;
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
            var operation = MakeRelocationOperation();

            operation.RelocateItem(operation.BaseDocument.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", 1);

            Assert.AreEqual(2, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantRelocateMoreThanOnDocument()
        {
            var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(operation.BaseDocument.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", 2);
            });

            Assert.AreEqual(0, operation.PendingAllocations.Count);
        }

        [Test]
        public void CantRelocateNonPresentOnDocProduct()
        {
            var product = new Product();
            var operation = MakeRelocationOperation();

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
            var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(operation.BaseDocument.Lines[0].Product, 
                    "LOC-001-01-1", "LOC-001-01-2", quantity);
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

        [Test]
        public void PerformRegisterAllocations()
        {
            var items = new List<Allocation>();
            var mock = new Mock<IAllocationService>();
            mock.Setup(m=>m.RegisterAllocation(It.IsAny<Allocation>()))
                .Callback((Allocation alloc)=>{ items.Add(alloc); });

            mock.Setup(m=>m.RegisterDeallocation(It.IsAny<Allocation>()))
                .Callback((Allocation alloc)=>{ items.Add(alloc); });

            var operation = MakeRelocationOperation();
            operation.RelocateItem(operation.BaseDocument.Lines[0].Product, 
                "LOC-001-01-1", "LOC-001-01-2", 1);

            operation.Perform(mock.Object);

            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(-1, items[0].Quantity);
            Assert.AreEqual(1, items[1].Quantity);
        }

        [Test]
        public void SourceLocationCantBeDestination()
        {
            var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(operation.BaseDocument.Lines[0].Product, 
                    "LOC-001-01-1", "LOC-001-01-1", 1);
            });
        }

        protected RelocationOperation MakeRelocationOperation()
        {
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            return operation;
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