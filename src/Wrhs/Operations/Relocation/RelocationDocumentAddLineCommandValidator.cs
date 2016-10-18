using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentAddLineCommandValidator : Validator<RelocationDocumentBuilderAddLineCommand>
    {
        IRepository<Product> productRepository;
        
        public RelocationDocumentAddLineCommandValidator(IRepository<Product> productRepository) 
        {
            this.productRepository = productRepository;
        }

        public override IEnumerable<ValidationResult> Validate(RelocationDocumentBuilderAddLineCommand command)
        {
            return null;
        }
    }
}