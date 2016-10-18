using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentAddLineCommandValidator : DocumentBuilderAddLineCommandValidator
    {
        public RelocationDocumentAddLineCommandValidator(IRepository<Product> productRepository) 
            : base(productRepository)
        {
        }

        public override IEnumerable<ValidationResult> Validate(DocumentBuilderAddLineCommand command)
        {
            return base.Validate(command);
        }
    }
}