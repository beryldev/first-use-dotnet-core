using System;
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


    
    }
}