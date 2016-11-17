using System;
using System.Collections.Generic;
using System.Linq;

namespace Wrhs.Operations.Delivery
{

    public class DeliveryOperation : IOperation
    {
        List<Allocation> pendingAllocations = new List<Allocation>();
        
        public List<Allocation> PendingAllocations { get { return pendingAllocations.ToList();} }

        public DeliveryDocument BaseDocument 
        { 
           get { return baseDocument; }
        }

        public DeliveryDocument BaseDeliveryDocument
        {
            get 
            { 
                return baseDocument.GetType() == typeof(DeliveryDocument) ? (DeliveryDocument)baseDocument : null; 
            }
            set { baseDocument = value; }
        }

        DeliveryDocument baseDocument;

        public DeliveryOperation() { }

        public DeliveryOperation(DeliveryOperation.State state)
        {
            baseDocument = state.BaseDocument;
        }

        public OperationResult Perform(IAllocationService allocService)
        {
            if(baseDocument == null)
                throw new InvalidOperationException("Can't perform operation without base document");

            var result = new OperationResult();

            if(baseDocument.Lines.Count == 0)
            {
                result.Status = OperationResult.ResultStatus.Error;
                result.ErrorMessages.Add("Base document is empty");
            }

            if(!CheckAllocations())
            {
                result.Status = OperationResult.ResultStatus.Error;
                result.ErrorMessages.Add("Exists non allocated items");
            }
            
            if(result.Status == OperationResult.ResultStatus.Ok)
            {
                foreach(var allocation in pendingAllocations)
                    allocService.RegisterAllocation(allocation);
            }                  

            return result;
        }

        public void SetBaseDocument(DeliveryDocument delivery)
        {
            baseDocument = delivery;
        }

        public void AllocateItem(DeliveryDocumentLine item, decimal quantity, string location)
        {
            if(String.IsNullOrWhiteSpace(location))
                throw new InvalidOperationException("Wrong location address (empty)");

            if(quantity <= 0)
                return;

            if(baseDocument !=null)
            {
                var onDocument = baseDocument.Lines
                    .Where(l=>l.Product.Code.Equals(item.Product.Code))
                    .Sum(l=>l.Quantity);

                var allocated = pendingAllocations
                    .Where(a=>a.Product.Code.Equals(item.Product.Code))
                    .Sum(a=>a.Quantity);

                if(allocated + quantity > onDocument)
                    throw new InvalidOperationException("Can't allocate more than on document");
            }
           
            var allocation = new Allocation
            {
                Product = item.Product,
                Location = location,
                Quantity = quantity
            };
     
            pendingAllocations.Add(allocation);
        }

        public State ReadState()
        {
            if(BaseDocument == null)
                return new State();
                
            var state = new State
            {
                BaseDocument = new DeliveryDocument
                {
                    Id = BaseDocument.Id,
                    FullNumber = BaseDocument.FullNumber,
                    Remarks = BaseDocument.Remarks,
                    IssueDate = BaseDocument.IssueDate
                },
                PendingAllocations = PendingAllocations.ToArray()
            };
            state.BaseDocument.Lines.AddRange(BaseDocument.Lines);

            return state;
        }
        
        protected bool CheckAllocations()
        {
            var toAllocate = baseDocument.Lines.Sum(item=>item.Quantity);
            var allocated = pendingAllocations.Sum(item=>item.Quantity);

            return toAllocate == allocated;
        }


        public class State
        {
            public DeliveryDocument BaseDocument { get; set; }

            public IEnumerable<Allocation> PendingAllocations { get; set; }
        }
    }
}