using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Operations.Delivery
{
    public class DeliveryDocumentBuilder
    {
        public event EventHandler<IEnumerable<ValidationResult>> OnAddLineFail;

        List<DeliveryDocumentLine> lines = new List<DeliveryDocumentLine>();

        IRepository<Product> productRepository;

        DeliveryDocumentBuilderValidator validator;

        public IEnumerable<DeliveryDocumentLine> Lines { get { return lines.ToArray(); } }

        public DeliveryDocumentBuilder(IRepository<Product> productRepository,
            DeliveryDocumentBuilderValidator validator)
        {
            this.productRepository = productRepository;
            this.validator = validator;
        }
        
        public DeliveryDocument Build()
        {
            var document = new DeliveryDocument();
            document.Lines.AddRange(lines);

            return document;
        }

        public void AddLine(int productId, decimal quantity)
        {
            var validationResults = validator.ValidateAddLine(productId, quantity);
            if(validationResults.Count() > 0)
            {
                //OnAddLineFail?.Invoke(this, validationResults);
                return;
            }
            
            lines.Add(new DeliveryDocumentLine
            {
                Id = lines.Count+1,
                Product = productRepository.GetById(productId),
                Quantity = quantity
            });          
        }

        public void RemoveLine(DeliveryDocumentLine line)
        {
            lines.Remove(line);
        }

        public void UpdateLine(DeliveryDocumentLine line)
        {
            var lineToUpdate = lines
                .Where(item=>item.Id == line.Id)
                .FirstOrDefault();
            
            var indexOfLine = lines.IndexOf(lineToUpdate);
            lines.RemoveAt(indexOfLine);
            lines.Insert(indexOfLine, line);
        }
    }
}