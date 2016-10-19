using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocDocAddLineCmdValidator : DocBuilderAddLineCmdValidator, IValidator<RelocDocBuilderAddLineCmd>
    {
        public RelocDocAddLineCmdValidator(IRepository<Product> productRepository) : base(productRepository)
        {
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

            return result;
        }
    }
}