using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Products
{
    public class UpdateProductCommandValidator : IValidator<UpdateProductCommand>
    {
        IRepository<Product> productRepository;

        public UpdateProductCommandValidator(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public IEnumerable<ValidationResult> Validate(UpdateProductCommand command)
        {
            var result = new List<ValidationResult>();

            if(String.IsNullOrWhiteSpace(command.Code))
                result.Add(new ValidationResult("Code", "Invalid product code. Product code can't be empty."));

            if(String.IsNullOrWhiteSpace(command.Name))
                result.Add(new ValidationResult("Name", "Invalid product name. Product name can't be empty"));

            if(productRepository.GetById(command.ProductId) == null)
                result.Add(new ValidationResult("ProductId", $"Product with id: {command.ProductId} does not exist."));

            return result;
        }
    }
}