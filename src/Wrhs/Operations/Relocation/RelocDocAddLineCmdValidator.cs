using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocDocAddLineCmdValidator : DocBuilderAddLineCmdValidator, IValidator<RelocDocBuilderAddLineCmd>
    {
        IWarehouse warehouse;

        public RelocDocAddLineCmdValidator(IRepository<Product> productRepository, IWarehouse warehouse) : base(productRepository)
        {
            this.warehouse = warehouse;
        }

        public IEnumerable<ValidationResult> Validate(RelocDocBuilderAddLineCmd command)
        {
            var result = (List<ValidationResult>)base.Validate((IDocBuilderAddLineCmd)command);

            if(String.IsNullOrWhiteSpace(command.From))
                result.Add(new ValidationResult("From", "From address can't be empty."));

            if(String.IsNullOrWhiteSpace(command.To))
                result.Add(new ValidationResult("To", "To address can't be empty."));

            if(command.From != null && command.From.Equals(command.To, StringComparison.OrdinalIgnoreCase))
                result.AddRange(new ValidationResult[]
                {
                    new ValidationResult("From", "Invalid from address. From can't be equal to."),
                    new ValidationResult("To", "Invalid to address. To can't be equal from."),
                });

            if(result.Count > 0)
                return result;

            var product = productRepository.GetById(command.ProductId);
            var stocks = warehouse.CalculateStocks(product.Code);
            var quantity = stocks
                .Where(s=>s.Location.Equals(command.From, StringComparison.OrdinalIgnoreCase))
                .Sum(s=>s.Quantity);
            if(command.Quantity > quantity)
                result.Add(new ValidationResult("Quantity", "Ivalid quantity. Try relocate more than at current location."));

            return result;
        }
    }
}