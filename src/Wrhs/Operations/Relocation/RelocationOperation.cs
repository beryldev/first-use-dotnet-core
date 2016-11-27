using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationOperation : IOperation
    {
        public RelocationDocument BaseDocument
        {
            get { return baseDocument; }
        }

        public RelocationDocument BaseRelocationDocument
        {
            get 
            { 
                return baseDocument.GetType() == typeof(RelocationDocument) ? 
                    (RelocationDocument)baseDocument : null; 
            }
            set { baseDocument = value; }
        }

        //Returns merged pending allocations and pending deallocations
        public List<Allocation> PendingAllocations
        {
            get 
            { 
                var result = pendingAllocations.ToList();
                result.AddRange(pendingDeallocations.ToList());
                return result;
            }
        }

        RelocationDocument baseDocument;

        List<Allocation> pendingAllocations = new List<Allocation>();

        List<Allocation> pendingDeallocations = new List<Allocation>();

        public RelocationOperation() { }

        public RelocationOperation(OperationState<RelocationDocument> state)
        {
            baseDocument = state.BaseDocument;
            pendingAllocations = state.PendingAllocations.Where(r => r.Quantity > 0).ToList();
            pendingDeallocations = state.PendingAllocations.Where(r => r.Quantity < 0).ToList();

            state.BaseDocument = null;
            state.PendingAllocations = null;
        }

        public void SetBaseDocument(RelocationDocument doc)
        {
            baseDocument = doc;
        }

        public OperationResult Perform(IAllocationService allocService)
        {
            if(baseDocument == null)
                throw new InvalidOperationException("Must set base document");

            if(!CheckAllocations())
                throw new InvalidOperationException($"Exists non relocated items.");
                

            if(pendingAllocations.Count != pendingDeallocations.Count)
                throw new InvalidOperationException("Pending allocations and pending deallocations integrity failed");

            for(var i=0; i<pendingAllocations.Count; i++)
            {
                allocService.RegisterDeallocation(pendingDeallocations[i]);
                allocService.RegisterAllocation(pendingAllocations[i]);
            }

            return new OperationResult();
        }

        public void RelocateItem(Product product, string from, string to, decimal quantity)
        {
            ValidateRelocation(product, quantity, from, to);

            var line = ((RelocationDocument)baseDocument).Lines
                .Where(item=>item.Product.Code == product.Code
                    && ((RelocationDocumentLine)item).From.Equals(from)
                    &&((RelocationDocumentLine)item).To.Equals(to))
                .FirstOrDefault();

            if(quantity > line.Quantity)
                throw new ArgumentException("Invalid quantity. Cant relocate more than on document");

            var allocFrom = new Allocation(){Product = product, Location = from, Quantity = quantity*(-1) };
            var allocTo = new Allocation(){ Product = product, Location = to, Quantity = quantity };
            pendingDeallocations.Add(allocFrom);
            pendingAllocations.Add(allocTo);
        }

        protected void ValidateRelocation(Product product, decimal quantity, string from, string to)
        {
            if(((RelocationDocument)baseDocument).Lines.Where(item => item.Product.Code == product.Code).Count() == 0)
                throw new ArgumentException("Invalid product. Product not present on document.");

            if(quantity <= 0)
                throw new ArgumentException("Invalid quantity. Must be more than zero");

            if(from.Equals(to))
                throw new ArgumentException("Source location can't be destination");
        }

        protected bool CheckAllocations()
        {
            var toAllocate = baseDocument.Lines.Sum(item=>item.Quantity);
            var allocated = pendingAllocations.Sum(item=>item.Quantity);

            return toAllocate == allocated;
        }
    }
}