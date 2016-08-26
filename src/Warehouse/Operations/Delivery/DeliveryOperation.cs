using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Documents;
using Warehouse.Orders;
using Warehouse.Validation;

namespace Warehouse.Operations.Delivery
{
    public class DeliveryOperation
    {
        List<Allocation> pendingAllocations = new List<Allocation>();
        
        public List<Allocation> PendingAllocations { get { return pendingAllocations.ToList();} }

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

            var result = new DeliveryOperationResult();

            if(baseDocument.Lines.Count == 0)
            {
                result.Status = DeliveryOperationResult.ResultStatus.Error;
                result.ErrorMessages.Add("Base document is empty");
            }
            
            return result;
        }

        public void SetBaseDocument(Order order)
        {
            baseDocument = order;
        }

        public void SetBaseDocument(DeliveryDocument delivery)
        {
            baseDocument = delivery;
        }

        public void AllocateItem(OrderLine item, decimal quantity, string location)
        {
            if(String.IsNullOrWhiteSpace(location))
                throw new InvalidOperationException("Wrong location address (empty)");

            if(quantity <= 0)
                return;

            var allocation = new Allocation
            {
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                SKU = item.SKU,
                EAN = item.EAN,
                Quantity = quantity,
                Location = location
            };

            
            pendingAllocations.Add(allocation);
        }
        
    }
}