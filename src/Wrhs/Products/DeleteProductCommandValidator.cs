using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Products
{
    public class DeleteProductCommandValidator : IValidator<DeleteProductCommand>
    {
        private readonly List<ValidationResult> results;

        public DeleteProductCommandValidator()
        {
            results = new List<ValidationResult>();
        }

        public IEnumerable<ValidationResult> Validate(DeleteProductCommand command)
        {
            if(command.ProductId <= 0)
                results.Add(new ValidationResult("ProductId", "Invalid product id."));
                
            return results;
        }
    }
}