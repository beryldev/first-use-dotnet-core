using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Products;

namespace Wrhs.Operations.Release
{
    public class ReleaseOperation : IOperation
    {
        public ReleaseDocument BaseDocument { get { return baseDocument; } }

        public ReleaseDocument BaseReleaseDocument
        {
            get
            {
                return baseDocument.GetType().Equals(typeof(ReleaseDocument)) ?
                    (ReleaseDocument)baseDocument : null;
            }

            set { baseDocument = value; }
        }

        public List<Allocation> PendingAllocations
        {
            get { return pendingAllocations.ToList(); }
        }
        
        ReleaseDocument baseDocument;

        List<Allocation> pendingAllocations = new List<Allocation>();


        public ReleaseOperation() { }

        public ReleaseOperation(OperationState<ReleaseDocument> state)
        {
            baseDocument = state.BaseDocument;
            pendingAllocations = state.PendingAllocations.ToList();

            state.BaseDocument = null;
            state.PendingAllocations = null;
        }

        public void ReleaseItem(Product product, string location, decimal quantity)
        {

           VerifyRelease(product, location, quantity);

            var alloc = new Allocation
            {
                Product = product,
                Location = location,
                Quantity = quantity
            };

            pendingAllocations.Add(alloc);
        }

        public OperationResult Perform(IAllocationService allocService)
        {
            if(baseDocument == null)
                throw new InvalidOperationException("Can't perform operation without base document");

            if(pendingAllocations.Sum(item=>item.Quantity) < baseDocument.Lines.Sum(item=>item.Quantity))
                throw new InvalidOperationException("Can't perform release operation. Exists not release resources.");

            for(var i=0; i<pendingAllocations.Count; i++)
            {
                pendingAllocations[i].Quantity *= -1;
                allocService.RegisterDeallocation(pendingAllocations[i]);
            }
            
            return null;
        }

        public void SetBaseDocument(ReleaseDocument document)
        {
            baseDocument = document;
        }

        protected void VerifyRelease(Product product, string location , decimal quantity)
        {
            if(quantity <= 0)
                throw new ArgumentException("Can't release zero or less");
                
             var lines = baseDocument.Lines
                .Where(item=>item.Product.Code == product.Code);
            
            if(lines.Count() == 0)
                throw new ArgumentException("Product not exists at document");

            var line = lines.Where(item=>((ReleaseDocumentLine)item).Location.Equals(location))
                .FirstOrDefault();

            if(line == null)
                throw new ArgumentException("Location not exists at document");

            quantity += pendingAllocations.Where(item=>item.Product.Code == product.Code
                && item.Location.Equals(location)).Sum(item=>item.Quantity);


            if(quantity > line.Quantity)
                throw new ArgumentException("Can't release more than at document");
        }
    }
}