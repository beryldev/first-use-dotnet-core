using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Products
{
    public class DeleteProductCommandValidator : IValidator<DeleteProductCommand>
    {
        IRepository<Product> productRepository;

        public DeleteProductCommandValidator(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public IEnumerable<ValidationResult> Validate(DeleteProductCommand command)
        {
            var result = new List<ValidationResult>();

            if(command.ProductId < 0)
            {
                result.Add(new ValidationResult("ProductId", "Invalid ProductId, can't be negative"));
                return result;
            }

            var product = productRepository.GetById(command.ProductId);
            if(product == null)
                result.Add(new ValidationResult("ProductId", "Invalid ProductId, product does not exists"));

            return result;
        }
    }
}