using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using Warehouse.Documents;
using Warehouse.Operations.Delivery;
using Warehouse.Orders;
using Warehouse.Operations;

namespace Warehouse.Tests
{
    [TestFixture]
    public class DeliveryOperationTests
    {
        [Test]
        public void OperationCanBasedOnOrder()
        {
            var order = new Order();
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);

            operation.SetBaseDocument(order);

            Assert.AreEqual(order, operation.BaseDocument);
        }

        [Test]
        public void OperationCanBasedOnDeliveryDocument()
        {
            var document = new DeliveryDocument();
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);

            operation.SetBaseDocument(document);

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void CanAcccessDirectlyToBaseOrder()
        {
            var document = new Order();
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            operation.BaseOrder = document;

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void CanAccessDirectlyToBaseDeliveryDocument()
        {
            var document = new DeliveryDocument();
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);

            operation.BaseDeliveryDocument = document;

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void BaseDocumentIsAlwaysLastSetDocumentOrderThenDeliveryDoc()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = new Order();
            var deliveryDoc = new DeliveryDocument();

            operation.SetBaseDocument(order);
            operation.SetBaseDocument(deliveryDoc);

            Assert.AreEqual(deliveryDoc, operation.BaseDocument);

        }

        [Test]
        public void BaseDocumentIsAlwaysLastSetDocumentDeliveryDocThenOrder()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = new Order();
            var deliveryDoc = new DeliveryDocument();

            operation.SetBaseDocument(deliveryDoc);
            operation.SetBaseDocument(order);

            Assert.AreEqual(order, operation.BaseDocument);
        }

        [Test]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(); 
            });
        }

        [Test]
        public void PerformOperationReturnOperationResult()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = new Order();
            operation.SetBaseDocument(order);
            
            var result = operation.Perform();

            Assert.IsNotNull(result);
        }

        [Test]
        public void PerformReturnResultWithErrorStatusWhenBaseDocumentIsEmpty()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = new Order();
            operation.SetBaseDocument(order);

            var result = operation.Perform();

            Assert.AreEqual(DeliveryOperationResult.ResultStatus.Error, result.Status);
        }

        [Test]
        public void PerformReturnResultWithErrorMessageWhenBaseDocumentIsEmpty()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = new Order();
            operation.SetBaseDocument(order);

            var result = operation.Perform();

            CollectionAssert.Contains(result.ErrorMessages, "Base document is empty");
        }

        [Test]
        public void CanAllocateItemFromBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = PrepareOrder();

            var location = "I-100-10";
            var quantity = 5;
            operation.AllocateItem((OrderLine)order.Lines.First(), quantity, location);
            var allocation = operation.PendingAllocations.First();

            Assert.AreEqual(allocation.ProductName, order.Lines.First().ProductName);
            Assert.AreEqual(allocation.Quantity, quantity);
            Assert.AreEqual(allocation.Location, location);
        }

        [Test]
        public void WhenTryAllocateZeroQuantityThenNoEffect()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = PrepareOrder();

            var location = "I-100-10";
            var quantity = 0;
            operation.AllocateItem((OrderLine)order.Lines.First(), quantity, location);

            CollectionAssert.IsEmpty(operation.PendingAllocations);
        }

        [Test]
        public void ThrowsExceptionWhenTryAllocateToEmptyLocationAddress()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = PrepareOrder();

            var location = String.Empty;
            var quantity = 1;

            Assert.Throws<InvalidOperationException>(()=>
            {
                operation.AllocateItem((OrderLine)order.Lines.First(), quantity, location);
            });
        }

        [Test]
        public void ThrowsExceptionWhenTryAllocateMoreThanOnDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = PrepareOrder();
            operation.SetBaseDocument(order);

            var location = "LOC-001-01";
            var quantity = 3;

    
            operation.AllocateItem((OrderLine)order.Lines.First(), quantity, location);

            Assert.Throws<InvalidOperationException>(()=>
            {
                operation.AllocateItem((OrderLine)order.Lines.First(), quantity, location);
            });
        }

        [Test]
        public void CantPerformOperationWhenExistsNonAllocatedItems()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            var order = PrepareOrder();
            operation.BaseOrder = order;

            var result = operation.Perform();

            Assert.AreEqual(DeliveryOperationResult.ResultStatus.Error, result.Status);
            CollectionAssert.Contains(result.ErrorMessages, "Exists non allocated items");
        }

        [Test]
        public void RegisterAllocationOnSuccessPerform()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(mock.Object);
            operation.SetBaseDocument(PrepareOrder());
            operation.AllocateItem((OrderLine)operation.BaseDocument.Lines.First(), 5, "LOC-001-01");

            operation.Perform();

            mock.Verify(m=>m.RegisterAllocation(It.IsAny<Allocation>()), Times.Once());
        }

        protected Order PrepareOrder()
        {
            var order = new Order();
            order.Lines.Add(new OrderLine
            {
                ProductName = "Some product",
                ProductCode = "SPROD",
                EAN = "1234567890",
                SKU = "P12345",
                Quantity = 5,
                Remarks = ""
            });

            return order;
        }
    }
}