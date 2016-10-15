using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationOperation : IOperation
    {
        public Document BaseDocument
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

        Document baseDocument;

        List<Allocation> pendingAllocations = new List<Allocation>();

        List<Allocation> pendingDeallocations = new List<Allocation>();

        public void SetBaseDocument(RelocationDocument doc)
        {
            baseDocument = doc;
        }

        public OperationResult Perform(IAllocationService allocService)
        {
            if(baseDocument == null)
                throw new InvalidOperationException("Must set base document");

            if(!CheckAllocations())
                throw new InvalidOperationException("Exists non relocated items");

            if(pendingAllocations.Count != pendingDeallocations.Count)
                throw new InvalidOperationException("Pending allocations and pending deallocations integrity failed");

            for(var i=0; i<pendingAllocations.Count; i++)
            {
                allocService.RegisterDeallocation(pendingDeallocations[i]);
                allocService.RegisterAllocation(pendingAllocations[i]);
            }

            return null;
        }

        public void RelocateItem(Product product, string from, string to, decimal quantity)
        {
            ValidateRelocation(product, quantity, from, to);

            var line = ((RelocationDocument)baseDocument).Lines
                .Where(item=>item.Product.Equals(product)
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
            if(((RelocationDocument)baseDocument).Lines.Where(item => item.Product.Equals(product)).Count() == 0)
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