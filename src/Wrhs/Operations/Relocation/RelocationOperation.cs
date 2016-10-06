using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Documents;

namespace Wrhs.Operations.Relocation
{
    public class RelocationOperation
    {
        public Document BaseDocument
        {
            get { return baseDocument; }
        }

        public RelocationDocument BaseRelocationDocument
        {
            get 
            { 
                return baseDocument.GetType() == typeof(RelocationDocument) ? (RelocationDocument)baseDocument : null; 
            }
            set { baseDocument = value; }
        }

        public List<Allocation> PendingAllocations
        {
            get { return pendingAllocations.ToList(); }
        }

        Document baseDocument;

        List<Allocation> pendingAllocations = new List<Allocation>();

        public void SetBaseDocument(RelocationDocument doc)
        {
            baseDocument = doc;
        }

        public OperationResult Perform(IAllocationService allocService)
        {
            if(baseDocument == null)
                throw new InvalidOperationException("Must set base document");

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
            pendingAllocations.Add(allocFrom);
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

        

        //dodac inne bazujac na deliveryoperation
    }
}