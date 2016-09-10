using System;
using System.Linq;
using NUnit.Framework;
using Warehouse.Documents;
using Warehouse.Operations.Delivery;
using Warehouse.Orders;

namespace Warehouse.Tests
{
    [TestFixture]
    public class DeliveryOperationTests
    {
        [Test]
        public void OperationCanBasedOnOrder()
        {
            var order = new Order();
            var operation = new DeliveryOperation();

            operation.SetBaseDocument(order);

            Assert.AreEqual(order, operation.BaseDocument);
        }

        [Test]
        public void OperationCanBasedOnDeliveryDocument()
        {
            var document = new DeliveryDocument();
            var operation = new DeliveryOperation();

            operation.SetBaseDocument(document);

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void CanAcccessDirectlyToBaseOrder()
        {
            var document = new Order();
            var operation = new DeliveryOperation();

            operation.BaseOrder = document;

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void CanAccessDirectlyToBaseDeliveryDocument()
        {
            var document = new DeliveryDocument();
            var operation  = new DeliveryOperation();

            operation.BaseDeliveryDocument = document;

            Assert.AreEqual(document, operation.BaseDocument);
        }

        [Test]
        public void BaseDocumentIsAlwaysLastSetDocumentOrderThenDeliveryDoc()
        {
            var operation = new DeliveryOperation();
            var order = new Order();
            var deliveryDoc = new DeliveryDocument();

            operation.SetBaseDocument(order);
            operation.SetBaseDocument(deliveryDoc);

            Assert.AreEqual(deliveryDoc, operation.BaseDocument);

        }

        [Test]
        public void BaseDocumentIsAlwaysLastSetDocumentDeliveryDocThenOrder()
        {
            var operation = new DeliveryOperation();
            var order = new Order();
            var deliveryDoc = new DeliveryDocument();

            operation.SetBaseDocument(deliveryDoc);
            operation.SetBaseDocument(order);

            Assert.AreEqual(order, operation.BaseDocument);
        }

        [Test]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var operation = new DeliveryOperation();

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(); 
            });
        }

        [Test]
        public void PerformOperationReturnOperationResult()
        {
            var operation = new DeliveryOperation();
            var order = new Order();
            operation.SetBaseDocument(order);
            
            var result = operation.Perform();

            Assert.IsNotNull(result);
        }

        [Test]
        public void PerformReturnResultWithErrorStatusWhenBaseDocumentIsEmpty()
        {
            var operation = new DeliveryOperation();
            var order = new Order();
            operation.SetBaseDocument(order);

            var result = operation.Perform();

            Assert.AreEqual(DeliveryOperationResult.ResultStatus.Error, result.Status);
        }

        [Test]
        public void PerformReturnResultWithErrorMessageWhenBaseDocumentIsEmpty()
        {
            var operation = new DeliveryOperation();
            var order = new Order();
            operation.SetBaseDocument(order);

            var result = operation.Perform();

            CollectionAssert.Contains(result.ErrorMessages, "Base document is empty");
        }

        [Test]
        public void CanAllocateItemFromBaseDocument()
        {
            var operation = new DeliveryOperation();
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
            var operation = new DeliveryOperation();
            var order = PrepareOrder();

            var location = "I-100-10";
            var quantity = 0;
            operation.AllocateItem((OrderLine)order.Lines.First(), quantity, location);

            CollectionAssert.IsEmpty(operation.PendingAllocations);
        }

        [Test]
        public void ThrowsExceptionWhenTryAllocateToEmptyLocationAddress()
        {
            var operation = new DeliveryOperation();
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
            var operation = new DeliveryOperation();
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
            var operation = new DeliveryOperation();
            var order = PrepareOrder();
            operation.BaseOrder = order;

            var result = operation.Perform();

            Assert.AreEqual(DeliveryOperationResult.ResultStatus.Error, result.Status);
            CollectionAssert.Contains(result.ErrorMessages, "Exists non allocated items");
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