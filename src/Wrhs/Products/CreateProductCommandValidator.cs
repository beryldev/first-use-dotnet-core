using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Products.Core;

namespace Wrhs.Products.Products
{
    public class CreateProductCommandValidator : IValidator<CreateProductCommand>
    {
        IRepository<Product> productRepository;

        public CreateProductCommandValidator(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public IEnumerable<ValidationResult> Validate(CreateProductCommand command)
        {
            var result = new List<ValidationResult>();

            if(String.IsNullOrWhiteSpace(command.Code))
                result.Add(new ValidationResult("Code", "Product code can't be empty"));

            if(String.IsNullOrWhiteSpace(command.Name))
                result.Add(new ValidationResult("Name", "Product name can't be empty"));

            var items = productRepository.Get()
                .Where(p=>p.Code.Equals(command.Code, StringComparison.OrdinalIgnoreCase)
                    || p.EAN.Equals(command.EAN, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if(items.Where(item=>item.Code==command.Code.ToUpper()).Count() > 0)
                result.Add(new ValidationResult("Code", "Product with this code already exists"));

            if(items.Where(item=>item.EAN==command.EAN.ToUpper()).Count() > 0)
                result.Add(new ValidationResult("EAN", "Product with this EAN already exists"));

            return result;
        }
    }
}