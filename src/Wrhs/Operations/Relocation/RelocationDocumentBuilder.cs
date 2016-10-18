using System;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentBuilder : DocumentBuilder<RelocationDocument, RelocationDocumentLine>
    {
        IRepository<Product> productRepository;

        public RelocationDocumentBuilder(IRepository<Product> productRepository, IValidator<DocumentBuilderAddLineCommand> addLineValidator) 
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

        protected override RelocationDocumentLine CommandToDocumentLine(DocumentBuilderAddLineCommand command)
        {
            throw new NotImplementedException();
        }

        protected override DocumentBuilderAddLineCommand DocumentLineToAddLineCommand(RelocationDocumentLine line)
        {
            throw new NotImplementedException();
        }
    }
}