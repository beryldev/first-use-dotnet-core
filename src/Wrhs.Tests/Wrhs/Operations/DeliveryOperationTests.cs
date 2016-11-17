using System;
using System.Linq;
using Moq;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests
{
    public class DeliveryOperationTests
    { 
        [Fact]
        public void OperationCanBasedOnDeliveryDocument()
        {
            var document = new Operations.Delivery.DeliveryDocument();
            var operation = new DeliveryOperation();

            operation.SetBaseDocument(document);

            Assert.Equal(document, operation.BaseDocument);
        }

        [Fact]
        public void CanAccessDirectlyToBaseDeliveryDocument()
        {
            var document = new Operations.Delivery.DeliveryDocument();
            var operation = new DeliveryOperation();

            operation.BaseDeliveryDocument = document;

            Assert.Equal(document, operation.BaseDocument);
        }

        [Fact]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(mock.Object); 
            });
        }

        [Fact]
        public void PerformOperationReturnOperationResult()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            var document = new DeliveryDocument();
            operation.SetBaseDocument(document);
            
            var result = operation.Perform(mock.Object);

            Assert.NotNull(result);
        }

        [Fact]
        public void PerformReturnResultWithErrorStatusWhenBaseDocumentIsEmpty()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            var document = new Operations.Delivery.DeliveryDocument();
            operation.SetBaseDocument(document);

            var result = operation.Perform(mock.Object);

            Assert.Equal(OperationResult.ResultStatus.Error, result.Status);
        }

        [Fact]
        public void PerformReturnResultWithErrorMessageWhenBaseDocumentIsEmpty()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            var document = new Operations.Delivery.DeliveryDocument();
            operation.SetBaseDocument(document);

            var result = operation.Perform(mock.Object);

            Assert.Contains("Base document is empty", result.ErrorMessages);
        }

        [Fact]
        public void CanAllocateItemFromBaseDocument()
        {
            var operation = new DeliveryOperation();
            var document = CreateDeliveryDocument();

            var location = "I-100-10";
            var quantity = 5;
            operation.AllocateItem(document.Lines.First(), quantity, location);
            var allocation = operation.PendingAllocations.First();

            Assert.Equal(allocation.Product, document.Lines.First().Product);
            Assert.Equal(allocation.Quantity, quantity);
            Assert.Equal(allocation.Location, location);
        }

        [Fact]
        public void WhenTryAllocateZeroQuantityThenNoEffect()
        {
            var operation = new DeliveryOperation();
            var document = CreateDeliveryDocument();

            var location = "I-100-10";
            var quantity = 0;
            operation.AllocateItem(document.Lines.First(), quantity, location);

            Assert.Empty(operation.PendingAllocations);
        }

        [Fact]
        public void ThrowsExceptionWhenTryAllocateToEmptyLocationAddress()
        {
            var operation = new DeliveryOperation();
            var document = CreateDeliveryDocument();

            var location = String.Empty;
            var quantity = 1;

            Assert.Throws<InvalidOperationException>(()=>
            {
                operation.AllocateItem(document.Lines.First(), quantity, location);
            });
        }

        [Fact]
        public void ThrowsExceptionWhenTryAllocateMoreThanOnDocument()
        {
            var operation = new DeliveryOperation();
            var document = CreateDeliveryDocument();
            operation.SetBaseDocument(document);

            var location = "LOC-001-01";
            var quantity = 3;

    
            operation.AllocateItem(document.Lines.First(), quantity, location);

            Assert.Throws<InvalidOperationException>(()=>
            {
                operation.AllocateItem(document.Lines.First(), quantity, location);
            });
        }

        [Fact]
        public void CantPerformOperationWhenExistsNonAllocatedItems()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            var document = CreateDeliveryDocument();
            operation.BaseDeliveryDocument = document;

            var result = operation.Perform(mock.Object);

            Assert.Equal(OperationResult.ResultStatus.Error, result.Status);
            Assert.Contains("Exists non allocated items", result.ErrorMessages);
        }

        [Fact]
        public void RegisterAllocationOnSuccessPerform()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            operation.SetBaseDocument(CreateDeliveryDocument());
            operation.AllocateItem(operation.BaseDocument.Lines.First(), 5, "LOC-001-01");

            operation.Perform(mock.Object);

            mock.Verify(m=>m.RegisterAllocation(It.IsAny<Allocation>()), Times.Once());
        }

        [Fact]
        public void ShouldReturnOperationStateOnReadStateAtFreshOperation()
        {   
            var operation = new DeliveryOperation();

            var state = operation.ReadState();

            Assert.IsType<DeliveryOperation.State>(state);
        }

        [Fact]
        public void ShouldReturnOperationStateOnReadState()
        {           
            var operation = CreateSimpleDeliveryOperation();

            var state = operation.ReadState();

            Assert.IsType<DeliveryOperation.State>(state);
            Assert.Equal(operation.BaseDocument.FullNumber, state.BaseDocument.FullNumber);
            Assert.Equal(1, state.BaseDocument.Lines.Count());
            Assert.Equal(1, state.PendingAllocations.Count());
        }

        [Fact]
        public void ShouldRecreateOperationFromState()
        {
            var operation = CreateSimpleDeliveryOperation();
            var state = new DeliveryOperation.State(operation);

            var recreated = new DeliveryOperation(state);

            Assert.Equal(state.BaseDocument.FullNumber, recreated.BaseDocument.FullNumber);
        }

        protected DeliveryOperation CreateSimpleDeliveryOperation()
        {
            var quantity = 1;
            var location = "LOC-001-01";
            var document = new DeliveryDocument(){ FullNumber = "DOC\001" };
            document.Lines.Add(new DeliveryDocumentLine()
            {
                Product = new Product{Code = "PROD1", Name = "Product 1", Id = 1},
                Quantity = 1
            });
            var operation = new DeliveryOperation();
            operation.SetBaseDocument(document);
            operation.AllocateItem(document.Lines.First(), quantity, location);

            return operation;
        }

        protected DeliveryDocument CreateDeliveryDocument()
        {
            var doc = new DeliveryDocument();


            doc.Lines.Add(new DeliveryDocumentLine
            {
                Product = new Product{ Name = "Some product", Code = "SPROD" },
                EAN = "1234567890",
                SKU = "P12345",
                Quantity = 5,
                Remarks = ""
            });

            return doc;
        }
    }
}