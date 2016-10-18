using System;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentBuilder 
        : DocumentBuilder<RelocationDocument, RelocationDocumentLine, RelocationDocumentBuilderAddLineCommand>
    {
        IRepository<Product> productRepository;

        public RelocationDocumentBuilder(IRepository<Product> productRepository, IValidator<RelocationDocumentBuilderAddLineCommand> addLineValidator) 
            : base(addLineValidator)
        {
            this.productRepository = productRepository;
        }

        public override RelocationDocument Build()
        {
            var document = new RelocationDocument();
            document.Lines.AddRange(lines);
            return document;
        }

        protected override RelocationDocumentLine CommandToDocumentLine(RelocationDocumentBuilderAddLineCommand command)
        {
            throw new NotImplementedException();
        }

        protected override RelocationDocumentBuilderAddLineCommand DocumentLineToAddLineCommand(RelocationDocumentLine line)
        {
            throw new NotImplementedException();
        }
    }
}