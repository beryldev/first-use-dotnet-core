using System;
using Warehouse.Documents;
using Warehouse.Orders;
using Warehouse.Validation;

namespace Warehouse.Operations.Delivery
{
    public class DeliveryOperation
    {

        public Document BaseDocument { get { return baseDocument; } }

        Document baseDocument; 
        
        public DeliveryOperationResult Perform()
        {
            return new DeliveryOperationResult();
        }

        public void SetBaseDocument(Order order)
        {
            baseDocument = order;
        }

        public void SetBaseDocument(DeliveryDocument delivery)
        {
            baseDocument = delivery;
        }
    }
}