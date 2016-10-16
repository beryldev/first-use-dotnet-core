using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Delivery
{
    public class DeliveryDocumentBuilder
    {
        public event EventHandler<IEnumerable<ValidationResult>> OnAddLineFail;

        List<DeliveryDocumentLine> lines = new List<DeliveryDocumentLine>();

        IRepository<Product> productRepository;

        IValidator<DocumentBuilderAddLineCommand> addLineValidator;

        IValidator<DocumentBuilderUpdateLineCommand> updateLineValidator;

        public IEnumerable<DeliveryDocumentLine> Lines { get { return lines.ToArray(); } }

        public DeliveryDocumentBuilder(IRepository<Product> productRepository,
            IValidator<DocumentBuilderAddLineCommand> addLineValidator,
            IValidator<DocumentBuilderUpdateLineCommand> updateLineValidator)
        {
            this.productRepository = productRepository;
            this.addLineValidator = addLineValidator;
            this.updateLineValidator = updateLineValidator;
        }
        
        public DeliveryDocument Build()
        {
            var document = new DeliveryDocument();
            document.Lines.AddRange(lines);

            return document;
        }

        public void AddLine(DocumentBuilderAddLineCommand command)
        {
            var validationResults = addLineValidator.Validate(command);
            if(validationResults.Count() > 0)
            {
                OnAddLineFail?.Invoke(this, validationResults);
                return;
            }
            
            lines.Add(new DeliveryDocumentLine
            {
                Id = lines.Count+1,
                Product = productRepository.GetById(command.ProductId),
                Quantity = command.Quantity
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