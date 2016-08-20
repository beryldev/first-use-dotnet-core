using System;
using Warehouse.Documents;
using Warehouse.Orders;
using Warehouse.Validation;

namespace Warehouse.Operations.Delivery
{
    public class DeliveryOperation
    {
        public Document BaseDocument 
        { 
           get { return baseDocument; }
        }

        public Order BaseOrder 
        { 
            get 
            { 
                return baseDocument.GetType() == typeof(Order) ? (Order)baseDocument : null; 
            } 
            set { baseDocument = value; }
        }

        public DeliveryDocument BaseDeliveryDocument
        {
            get 
            { 
                return baseDocument.GetType() == typeof(DeliveryDocument) ? (DeliveryDocument)baseDocument : null; 
            }
            set { baseDocument = value; }
        }

        Document baseDocument;
        
        public DeliveryOperationResult Perform()
        {
            if(baseDocument == null)
                throw new InvalidOperationException("Can't perform operation without base document");
                
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