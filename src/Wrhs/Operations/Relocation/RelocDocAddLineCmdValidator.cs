using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocDocAddLineCmdValidator : Validator<RelocDocBuilderAddLineCmd>
    {
        IRepository<Product> productRepository;
        
        public RelocDocAddLineCmdValidator(IRepository<Product> productRepository) 
        {
            this.productRepository = productRepository;
        }

        public override IEnumerable<ValidationResult> Validate(RelocDocBuilderAddLineCmd command)
        {
            return null;
        }
    }
}