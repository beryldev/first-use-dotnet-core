using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Operations.Delivery
{
    public class DeliveryDocumentBuilder
    {
        List<DeliveryDocumentLine> lines = new List<DeliveryDocumentLine>();

        IRepository<Product> productRepository;

        public IEnumerable<DeliveryDocumentLine> Lines { get { return lines.ToArray(); } }

        public DeliveryDocumentBuilder(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }
        
        public DeliveryDocument Build()
        {
            var document = new DeliveryDocument();
            document.Lines.AddRange(lines);

            return document;
        }

        public void AddLine(int productId, decimal quantity)
        {
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