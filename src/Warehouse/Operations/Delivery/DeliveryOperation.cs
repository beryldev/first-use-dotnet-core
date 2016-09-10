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

            if(!CheckAllocations())
            {
                result.Status = DeliveryOperationResult.ResultStatus.Error;
                result.ErrorMessages.Add("Exists non allocated items");
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

            if(baseDocument !=null)
            {
                var onDocument = baseDocument.Lines
                    .Where(l=>l.ProductCode.Equals(item.ProductCode))
                    .Sum(l=>l.Quantity);

                var allocated = pendingAllocations
                    .Where(a=>a.ProductCode.Equals(item.ProductCode))
                    .Sum(a=>a.Quantity);
                    
                if(allocated + quantity > onDocument)
                    throw new InvalidOperationException("Can't allocate more than on document");
            }
           
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
        
        protected bool CheckAllocations()
        {
            var toAllocate = baseDocument.Lines.Sum(item=>item.Quantity);
            var allocated = pendingAllocations.Sum(item=>item.Quantity);

            return toAllocate == allocated;
        }
    }
}