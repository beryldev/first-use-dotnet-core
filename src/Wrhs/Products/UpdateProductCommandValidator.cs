using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Products
{
    public class UpdateProductCommandValidator : CommandValidator<UpdateProductCommand>
    {
        private readonly IProductService productSrv;

        public UpdateProductCommandValidator(IProductService productSrv)
        {
            this.productSrv = productSrv;
        }

        public override IEnumerable<ValidationResult> Validate(UpdateProductCommand command)
        {
            if(String.IsNullOrWhiteSpace(command.Name))
                AddValidationResult("Name", "Product name can't be empty.");

            if(String.IsNullOrWhiteSpace(command.Code))
                AddValidationResult("Code", "Product code can't be empty.");

            if(!productSrv.CheckProductExists(command.ProductId))
                AddValidationResult("ProductId", $"Product with id: {command.ProductId} not exists.");

            ValidateUniqueNameAndCode(command);

            return results;
        }

        private void ValidateUniqueNameAndCode(UpdateProductCommand command)
        {
            var prodByName = productSrv.GetProductByName(command.Name);
            var prodByCode = productSrv.GetProductByCode(command.Code);

            if(prodByName != null && prodByName.Id != command.ProductId)
                AddValidationResult("Name", $"Other product with name: '{command.Name}' exists.");

            if(prodByCode != null && prodByCode.Id != command.ProductId)
                AddValidationResult("Name", $"Other product with code: '{command.Code}' exists.");
        }
    }

}