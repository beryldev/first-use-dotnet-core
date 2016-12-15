using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Products
{
    public class CreateProductCommandValidator : CommandValidator<CreateProductCommand>
    {
        private readonly IProductService productSrv;

        public CreateProductCommandValidator(IProductService productSrv)
        {
            this.productSrv = productSrv;
        }

        public override IEnumerable<ValidationResult> Validate(CreateProductCommand command)
        {
            if(String.IsNullOrWhiteSpace(command.Name))
                AddValidationResult("Name", "Product name can't be empty.");

            if(String.IsNullOrWhiteSpace(command.Code))
                AddValidationResult("Code", "Product code can't be empty.");

            if(productSrv.CheckProductExistsByName(command.Name))
                AddValidationResult("Name", $"Product with name: {command.Name} exists.");

            if(productSrv.CheckProductExistsByCode(command.Code))
                AddValidationResult("Code", $"Product with code: {command.Code} exists.");

            return results;
        }
    }
}