using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Documents
{
    public class DocumentBuilderAddLineCommandValidator : Validator<DocumentBuilderAddLineCommand>
    {
        IRepository<Product> productRepository;

        public DocumentBuilderAddLineCommandValidator(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public override IEnumerable<ValidationResult> Validate(DocumentBuilderAddLineCommand command)
        {
            var result = new List<ValidationResult>();

            if(command.Quantity <= 0)
                result.Add(new ValidationResult("Quantity", $"Invalid quantity: {command.Quantity}. Quantity must be greater than zero."));

            var product = productRepository.GetById(command.ProductId);
            if(product == null)
                result.Add(new ValidationResult("ProductId", $"Invalid product id: {command.ProductId}. Product not found."));

            return result;   
        }
    }

}