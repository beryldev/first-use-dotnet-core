using System;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentBuilder 
        : DocumentBuilder<RelocationDocument, RelocationDocumentLine, RelocDocBuilderAddLineCmd>
    {
        IRepository<Product> productRepository;

        public RelocationDocumentBuilder(IRepository<Product> productRepository, IValidator<RelocDocBuilderAddLineCmd> addLineValidator) 
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

        protected override RelocationDocumentLine CommandToDocumentLine(RelocDocBuilderAddLineCmd command)
        {
            throw new NotImplementedException();
        }

        protected override RelocDocBuilderAddLineCmd DocumentLineToAddLineCommand(RelocationDocumentLine line)
        {
            throw new NotImplementedException();
        }
    }
}