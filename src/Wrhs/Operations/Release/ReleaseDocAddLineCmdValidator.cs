using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Release
{
    public class ReleaseDocAddLineCmdValidator : DocBuilderAddLineCmdValidator, IValidator<ReleaseDocAddLineCmd>
    {
        IWarehouse warehouse;

        public ReleaseDocAddLineCmdValidator(IRepository<Product> productRepository, IWarehouse warehouse) 
            : base(productRepository)
        {
            this.warehouse = warehouse;
        }

        public IEnumerable<ValidationResult> Validate(ReleaseDocAddLineCmd command)
        {
            var result = base.Validate(command) as List<ValidationResult>;

            if(string.IsNullOrWhiteSpace(command.Location))
                result.Add(new ValidationResult("Location", "Invalid location. Location can't be empty."));

              if(result.Count > 0)
                return result;

            var product = productRepository.GetById(command.ProductId);
            var stocks = warehouse.CalculateStocks(product.Code);
            var quantity = stocks
                .Where(s=>s.Location.Equals(command.Location, StringComparison.OrdinalIgnoreCase))
                .Sum(s=>s.Quantity);
            if(command.Quantity > quantity)
                result.Add(new ValidationResult("Quantity", "Ivalid quantity. Try release more than at current location."));

            return result;
        }
    }
}