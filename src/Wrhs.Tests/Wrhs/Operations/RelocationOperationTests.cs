using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Operations;
using Wrhs.Operations.Relocation;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests
{
    public class RelocationOperationTests
    {
        [Fact]
        public void OperationCanBasedOnRelocationDocument()
        {
            var relocationDoc = MakeRelocationDocument();
            var operation = new RelocationOperation();

            operation.SetBaseDocument(relocationDoc);

            Assert.Equal(relocationDoc, operation.BaseDocument);
        }

        [Fact]
        public void CanAccessDirectlyToBaseRelocationDocument()
        {
            var relocDoc = MakeRelocationDocument();
            var operation = new RelocationOperation();

            operation.BaseRelocationDocument = relocDoc;

            Assert.Equal(relocDoc, operation.BaseDocument);
        }

        [Fact]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new RelocationOperation();

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(mock.Object); 
            });
        }

        [Fact]
        public void RelocateItem()
        {
            var operation = MakeRelocationOperation();

            operation.RelocateItem(operation.BaseDocument.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", 1);

            Assert.Equal(2, operation.PendingAllocations.Count);
        }

        [Fact]
        public void CantRelocateMoreThanOnDocument()
        {
            var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(operation.BaseDocument.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", 2);
            });

            Assert.Equal(0, operation.PendingAllocations.Count);
        }

        [Fact]
        public void CantRelocateNonPresentOnDocProduct()
        {
            var product = new Product();
            var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(product, "LOC-001-01-1", "LOC-001-01-2", 1);
            });

            Assert.Equal(0, operation.PendingAllocations.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-0.01)]
        [InlineData(-1)]
        [InlineData(-9)]
        public void CantRelocateZeroOrLess(decimal quantity)
        {
            var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(operation.BaseDocument.Lines[0].Product, 
                    "LOC-001-01-1", "LOC-001-01-2", quantity);
            });

            Assert.Equal(0, operation.PendingAllocations.Count);
        }

        [Fact]
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

        [Fact]
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

            Assert.Equal(2, items.Count);
            Assert.Equal(-1, items[0].Quantity);
            Assert.Equal(1, items[1].Quantity);
        }

        [Fact]
        public void SourceLocationCantBeDestination()
        {
            var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(operation.BaseDocument.Lines[0].Product, 
                    "LOC-001-01-1", "LOC-001-01-1", 1);
            });
        }

        [Fact]
        public void ShouldRestoreValidOperationFromState()
        {
            var allocSrv = new Mock<IAllocationService>();
            var product = new Product{Code="PROD1", Name="Product 1"};
            var doc = new RelocationDocument{FullNumber = "RLC/001/2016"};
            doc.Lines.Add(new RelocationDocumentLine
            {
                Product = product,
                Quantity = 10,
                From = "LOC-001-01",
                To = "LOC-001-2"
            });

            var state = new OperationState<RelocationDocument>()
            {
                BaseDocument = doc,
                PendingAllocations = new List<Allocation>
                {
                    new Allocation { Product = product, Location = "LOC-001-01", Quantity = -10},
                    new Allocation { Product = product, Location = "LOC-001-02", Quantity = 10}
                }
            };

            var operation = new RelocationOperation(state);

            var result = operation.Perform(allocSrv.Object);
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWhenTryMoveFromInvalidFromLocation()
        {
             var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(operation.BaseDocument.Lines[0].Product, 
                    "LOC-001-01-99", "LOC-001-01-2", 1);
            });

            Assert.Equal(0, operation.PendingAllocations.Count);
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWhenTryMoveFromInvalidToLocation()
        {
             var operation = MakeRelocationOperation();

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RelocateItem(operation.BaseDocument.Lines[0].Product, 
                    "LOC-001-01-1", "LOC-001-01-99", 1);
            });

            Assert.Equal(0, operation.PendingAllocations.Count);
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