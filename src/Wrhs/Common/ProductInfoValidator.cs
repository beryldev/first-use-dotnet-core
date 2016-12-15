using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Common
{
    public class ProductInfoValidator : IValidator<IValidableProductInfo>
    {
        private readonly IProductService productSrv;
        private readonly List<ValidationResult> results;

        public ProductInfoValidator(IProductService productSrv)
        {
            this.productSrv = productSrv;
            results = new List<ValidationResult>();
        }

        public IEnumerable<ValidationResult> Validate(IValidableProductInfo command)
        {
            if(command.ProductId <= 0)
                AddValidationResult("ProductId", "Invalid product id.");

            if(!productSrv.CheckProductExists(command.ProductId))
                AddValidationResult("ProductId", $"Product with id: {command.ProductId} not exists.");

            if(command.Quantity <= 0)
                AddValidationResult("Quantity", "Ivalid quantity");

            return results;
        }

        private void AddValidationResult(string field, string message)
        {
            results.Add(new ValidationResult(field, message));
        }
    }
}