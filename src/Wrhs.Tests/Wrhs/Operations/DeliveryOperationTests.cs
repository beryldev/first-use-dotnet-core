using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;
using Wrhs.Orders;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class DeliveryOperationTests
    { 
        [Test]
        public void OperationCanBasedOnDeliveryDocument()
        {
            var document = new Operations.Delivery.DeliveryDocument();
            var operation = new DeliveryOperation();

            operation.SetBaseDocument(document);

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void CanAccessDirectlyToBaseDeliveryDocument()
        {
            var document = new Operations.Delivery.DeliveryDocument();
            var operation = new DeliveryOperation();

            operation.BaseDeliveryDocument = document;

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(mock.Object); 
            });
        }

        [Test]
        public void PerformOperationReturnOperationResult()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            var document = new DeliveryDocument();
            operation.SetBaseDocument(document);
            
            var result = operation.Perform(mock.Object);

            Assert.IsNotNull(result);
        }

        [Test]
        public void PerformReturnResultWithErrorStatusWhenBaseDocumentIsEmpty()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            var document = new Operations.Delivery.DeliveryDocument();
            operation.SetBaseDocument(document);

            var result = operation.Perform(mock.Object);

            Assert.AreEqual(OperationResult.ResultStatus.Error, result.Status);
        }

        [Test]
        public void PerformReturnResultWithErrorMessageWhenBaseDocumentIsEmpty()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            var document = new Operations.Delivery.DeliveryDocument();
            operation.SetBaseDocument(document);

            var result = operation.Perform(mock.Object);

            CollectionAssert.Contains(result.ErrorMessages, "Base document is empty");
        }

        [Test]
        public void CanAllocateItemFromBaseDocument()
        {
            var operation = new DeliveryOperation();
            var document = CreateDeliveryDocument();

            var location = "I-100-10";
            var quantity = 5;
            operation.AllocateItem(document.Lines.First(), quantity, location);
            var allocation = operation.PendingAllocations.First();

            Assert.AreEqual(allocation.Product, document.Lines.First().Product);
            Assert.AreEqual(allocation.Quantity, quantity);
            Assert.AreEqual(allocation.Location, location);
        }

        [Test]
        public void WhenTryAllocateZeroQuantityThenNoEffect()
        {
            var operation = new DeliveryOperation();
            var document = CreateDeliveryDocument();

            var location = "I-100-10";
            var quantity = 0;
            operation.AllocateItem(document.Lines.First(), quantity, location);

            CollectionAssert.IsEmpty(operation.PendingAllocations);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void CantPerformOperationWhenExistsNonAllocatedItems()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            var document = CreateDeliveryDocument();
            operation.BaseDeliveryDocument = document;

            var result = operation.Perform(mock.Object);

            Assert.AreEqual(OperationResult.ResultStatus.Error, result.Status);
            CollectionAssert.Contains(result.ErrorMessages, "Exists non allocated items");
        }

        [Test]
        public void RegisterAllocationOnSuccessPerform()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation();
            operation.SetBaseDocument(CreateDeliveryDocument());
            operation.AllocateItem(operation.BaseDocument.Lines.First(), 5, "LOC-001-01");

            operation.Perform(mock.Object);

            mock.Verify(m=>m.RegisterAllocation(It.IsAny<Allocation>()), Times.Once());
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